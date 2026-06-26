using Google.Cloud.Firestore;

namespace DoughBro.src.Models
{
    public class CategoryModel
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }
    }
}
