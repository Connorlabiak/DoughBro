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

        public async Task<string> AddManualTransaction(TransactionModel transaction, string userId)
        {
            DocumentReference docRef = _db.Collection("users").Document(userId).Collection("transactions").Document();

            transaction.Id = docRef.Id;

            await docRef.SetAsync(transaction);

            return docRef.Id;
        }
    }
}
