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

        public async Task<string> CreateAsync(TransactionModel transactionModel)
        {
            DocumentReference document = _db.Collection("Transactions").Document();
            transactionModel.Id = document.Id;
            await document.SetAsync(transactionModel);
            return document.Id;
        }

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
    }
}
