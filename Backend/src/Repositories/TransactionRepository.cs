

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

        public async Task CreateAsync(TransactionModel transactionModel)
        {
            DocumentReference reference = _db.Collection("Transactions").Document();
            transactionModel.Id = reference.Id;
            await reference.SetAsync(transactionModel);
        }

        public async Task<TransactionModel?> GetAsync(string id)
        {
            DocumentReference reference = _db.Collection("Transactions").Document(id);
            DocumentSnapshot snapshot = await reference.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<TransactionModel>();
            }
            return null;
        }
    }
}
