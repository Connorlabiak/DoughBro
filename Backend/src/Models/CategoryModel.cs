using Google.Cloud.Firestore;

namespace DoughBro.src.Models
{
    [FirestoreData]
    public class CategoryModel
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }

        [FirestoreProperty]
        public required string Name { get; set; }

        [FirestoreProperty]
        public required string Color { get; set; }
    }
}
