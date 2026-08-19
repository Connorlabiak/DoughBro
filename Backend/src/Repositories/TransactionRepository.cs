using DoughBro.src.Database;
using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using Google.Cloud.Firestore;

namespace DoughBro.src.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly FirestoreDb _db;

        public TransactionRepository(IDbProvider dbProvider)
        {
            _db = dbProvider.GetFirestoreDb();
        }

        public async Task SaveBatch(IEnumerable<TransactionModel> transactions, string userId)
        {
            const int maxFirestoreBatchSize = 490;
            var chunks = transactions.Chunk(maxFirestoreBatchSize);

            foreach (var chunk in chunks)
            {
                WriteBatch batch = _db.StartBatch();

                foreach (var transaction in chunk)
                {
                    DocumentReference docRef = _db.Collection("users").Document(userId).Collection("transactions").Document(transaction.Id);
                    batch.Set(docRef, transaction);
                }

                await batch.CommitAsync();
            }
        }

        public async Task DeleteBatch(IEnumerable<string> transactionIds, string userId)
        {
            const int maxFirestoreBatchSize = 490;
            var chunks = transactionIds.Chunk(maxFirestoreBatchSize);

            foreach (var chunk in chunks)
            {
                WriteBatch batch = _db.StartBatch();
                foreach (var transactionId in chunk)
                {
                    DocumentReference docRef = _db.Collection("users").Document(userId).Collection("transactions").Document(transactionId);
                    batch.Delete(docRef);
                }

                await batch.CommitAsync();
            }
        }

        public async Task<IEnumerable<TransactionModel>> GetAllTransactions(string userId, int limit)
        {
            QuerySnapshot snapshot = await _db.Collection("users").Document(userId).Collection("transactions")
                .OrderByDescending("Date")
                .Limit(limit)
                .GetSnapshotAsync();

            return snapshot.Documents.Select(doc => doc.ConvertTo<TransactionModel>());
        }

        public async Task UpdateTransactionAsync(string userId, string transactionId, TransactionModel transaction)
        {
            DocumentReference docRef = _db.Collection("users").Document(userId).Collection("transactions").Document(transactionId);
            await docRef.SetAsync(new Dictionary<string, object?>
            {
                ["Name"] = transaction.Name,
                ["Date"] = transaction.Date,
                ["Amount"] = (double)transaction.Amount,
                ["MerchantName"] = transaction.MerchantName,
                ["Description"] = transaction.Description,
                ["Category"] = transaction.Category,
            }, SetOptions.MergeAll);
        }

        public async Task<IEnumerable<TransactionModel>> GetTransactionsByCategoryAsync(string userId, string categoryId)
        {
            QuerySnapshot snapshot = await _db.Collection("users").Document(userId).Collection("transactions")
                .WhereEqualTo("Category", categoryId)
                .GetSnapshotAsync();

            return snapshot.Documents.Select(doc => doc.ConvertTo<TransactionModel>());
        }

        public async Task ClearCategoryAsync(string userId, string categoryId)
        {
            IEnumerable<TransactionModel> transactions = await GetTransactionsByCategoryAsync(userId, categoryId);

            foreach (TransactionModel[] chunk in transactions.Chunk(490))
            {
                WriteBatch batch = _db.StartBatch();
                foreach (TransactionModel transaction in chunk)
                {
                    DocumentReference transactionRef = _db.Collection("users").Document(userId)
                        .Collection("transactions").Document(transaction.Id);
                    batch.Update(transactionRef, "Category", FieldValue.Delete);
                }

                await batch.CommitAsync();
            }
        }
    }
}
