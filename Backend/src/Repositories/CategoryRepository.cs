using DoughBro.src.Database;
using DoughBro.src.Models;
using DoughBro.src.Repositories.Interfaces;
using Google.Cloud.Firestore;
using Grpc.Core;

namespace DoughBro.src.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FirestoreDb _db;

        public CategoryRepository(IDbProvider dbProvider)
        {
            _db = dbProvider.GetFirestoreDb();
        }

        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync(string userId)
        {
            QuerySnapshot snapshot = await _db.Collection("users").Document(userId).Collection("categories")
                .OrderBy("Name")
                .GetSnapshotAsync();

            return snapshot.Documents.Select(doc => doc.ConvertTo<CategoryModel>());
        }

        public async Task<CategoryModel?> AddCategoryAsync(string userId, CategoryModel category)
        {
            DocumentReference userRef = _db.Collection("users").Document(userId);
            DocumentReference categoryRef = userRef.Collection("categories").Document();
            DocumentReference colorUsageRef = userRef.Collection("category_color_usage").Document(category.ColorId);
            category.Id = categoryRef.Id;

            WriteBatch batch = _db.StartBatch();
            batch.Create(colorUsageRef, new Dictionary<string, object>
            {
                ["ColorId"] = category.ColorId,
                ["CreatedAt"] = Timestamp.GetCurrentTimestamp(),
            });
            batch.Set(categoryRef, category);

            try
            {
                await batch.CommitAsync();
                return category;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                return null;
            }
        }

        public async Task<ISet<string>> GetUsedCategoryColorIdsAsync(string userId)
        {
            CollectionReference colorUsageCollection = _db.Collection("users").Document(userId).Collection("category_color_usage");
            QuerySnapshot colorUsageSnapshot = await colorUsageCollection.GetSnapshotAsync();
            ISet<string> usedColorIds = colorUsageSnapshot.Documents
                .Select(doc => doc.Id)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (usedColorIds.Count > 0)
            {
                return usedColorIds;
            }

            IEnumerable<CategoryModel> categories = await GetCategoriesAsync(userId);
            return categories.Select(category => category.ColorId).ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        public async Task<bool> AddDefaultCategoriesAsync(string userId, IEnumerable<CategoryModel> categories)
        {
            DocumentReference userRef = _db.Collection("users").Document(userId);
            DocumentReference initializationRef = userRef.Collection("category_defaults").Document("initialized");
            WriteBatch batch = _db.StartBatch();
            batch.Create(initializationRef, new Dictionary<string, object>
            {
                ["CreatedAt"] = Timestamp.GetCurrentTimestamp(),
            });

            foreach (CategoryModel category in categories)
            {
                DocumentReference categoryRef = userRef.Collection("categories").Document();
                category.Id = categoryRef.Id;
                batch.Set(categoryRef, category);
                batch.Create(userRef.Collection("category_color_usage").Document(category.ColorId), new Dictionary<string, object>
                {
                    ["ColorId"] = category.ColorId,
                    ["CreatedAt"] = Timestamp.GetCurrentTimestamp(),
                });
            }

            try
            {
                await batch.CommitAsync();
                return true;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                return false;
            }
        }

        public async Task<CategoryModel?> UpdateCategoryAsync(string userId, string categoryId, CategoryModel category)
        {
            DocumentReference userRef = _db.Collection("users").Document(userId);
            DocumentReference categoryRef = userRef.Collection("categories").Document(categoryId);
            DocumentSnapshot snapshot = await categoryRef.GetSnapshotAsync();
            if (!snapshot.Exists)
            {
                return null;
            }

            CategoryModel existingCategory = snapshot.ConvertTo<CategoryModel>();
            category.Id = categoryId;
            WriteBatch batch = _db.StartBatch();
            batch.Update(categoryRef, new Dictionary<string, object>
            {
                ["Name"] = category.Name,
                ["ColorId"] = category.ColorId,
                ["Color"] = category.Color,
            });

            if (!string.Equals(existingCategory.ColorId, category.ColorId, StringComparison.OrdinalIgnoreCase))
            {
                batch.Create(userRef.Collection("category_color_usage").Document(category.ColorId), new Dictionary<string, object>
                {
                    ["ColorId"] = category.ColorId,
                    ["CreatedAt"] = Timestamp.GetCurrentTimestamp(),
                });
                batch.Delete(userRef.Collection("category_color_usage").Document(existingCategory.ColorId));
            }

            try
            {
                await batch.CommitAsync();
                return category;
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.AlreadyExists)
            {
                return null;
            }
        }

        public async Task<bool> DeleteCategoryAsync(string userId, string categoryId)
        {
            DocumentReference userRef = _db.Collection("users").Document(userId);
            DocumentReference categoryRef = userRef.Collection("categories").Document(categoryId);
            DocumentSnapshot snapshot = await categoryRef.GetSnapshotAsync();
            if (!snapshot.Exists)
            {
                return false;
            }

            CategoryModel category = snapshot.ConvertTo<CategoryModel>();
            WriteBatch batch = _db.StartBatch();
            batch.Delete(categoryRef);
            batch.Delete(userRef.Collection("category_color_usage").Document(category.ColorId));
            await batch.CommitAsync();
            return true;
        }
    }
}
