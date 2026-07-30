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
            const int maxFirestoreBatchSize = 500;
            var chunks = transactions.Chunk(maxFirestoreBatchSize);

            foreach (var chunk in chunks)
            {
                WriteBatch batch = _db.StartBatch();

                foreach (var transaction in chunk)
                {
                    DocumentReference docRef = _db.Collection("users").Document(userId).Collection("transactions").Document();
                    transaction.Id = docRef.Id;
                    batch.Set(docRef, transaction);
                }
                await batch.CommitAsync();
            }
        }

        /*
        public async Task UpdateCategoryAsync(string transactionId, CategoryModel categoryModel)
        {
            DocumentReference document = _db.Collection("Transaction").Document(transactionId);
            await document.UpdateAsync("Category", categoryModel.Name);
        }

        public async Task<TransactionModel?> GetAsync(string id)
        {
            DocumentReference document = _db.Collection("Transactions").Document(id);
            DocumentSnapshot snapshot = await document.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<TransactionModel>();
            }
            return null;
        }
        */
    }
}
