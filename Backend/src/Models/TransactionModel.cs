using Google.Cloud.Firestore;

namespace DoughBro.src.Models
{
    [FirestoreData]
    public class TransactionModel
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string UserId { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Name { get; set; } = string.Empty;

        [FirestoreProperty]
        public string Date { get; set; } = string.Empty;

        [FirestoreProperty]
        public double Amount { get; set; }

        [FirestoreProperty]
        public string? Description { get; set; }

        [FirestoreProperty]
        public string? category { get; set; }
    }
}
