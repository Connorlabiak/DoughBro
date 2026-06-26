using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using Google.Cloud.Firestore;
using System.Data;

namespace DoughBro.src.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FirestoreDb _db;

        public CategoryRepository(IDbProvider dbProvider)
        {
            _db = dbProvider.GetFirestoreDb();
        }

        public async Task<string> CreateAsync(CategoryModel categoryModel)
        {
            DocumentReference document = _db.Collection("Categories").Document();
            categoryModel.Id = document.Id;
            await document.SetAsync(categoryModel);
            return document.Id;
        }

        public async Task<CategoryModel?> FindByNameAsync(string name)
        {
            CollectionReference collection = _db.Collection("Categories");
            QuerySnapshot snapshot = await collection.WhereEqualTo("Name", name).GetSnapshotAsync();
            if (snapshot.Count == 1)
                return snapshot.First().ConvertTo<CategoryModel>();
            else
                return null;
        }
    }
}
