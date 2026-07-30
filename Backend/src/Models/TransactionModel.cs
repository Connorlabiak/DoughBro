using Google.Cloud.Firestore;

namespace DoughBro.src.Models
{
    [FirestoreData]
    public class TransactionModel
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string? PlaidTransactionId { get; set; }

        [FirestoreProperty]
        public required string UserId { get; set; }

        [FirestoreProperty]
        public required string Name { get; set; }

        [FirestoreProperty]
        public required string Date { get; set; }

        [FirestoreProperty]
        public required decimal Amount { get; set; }

        [FirestoreProperty]
        public string? Description { get; set; }

        [FirestoreProperty]
        public string? Category { get; set; }

        [FirestoreProperty]
        public required bool IsPending { get; set; }
    }
}
