using DoughBro.src.Database;
using Google.Cloud.Firestore;

namespace DoughBro.src.Models
{
    [FirestoreData]
    public class TransactionModel
    {
        [FirestoreDocumentId]
        public string? Id { get; set; }

        [FirestoreProperty]
        public required string Origin { get; set; }

        [FirestoreProperty]
        public required string UserId { get; set; }

        [FirestoreProperty]
        public required string Name { get; set; }

        [FirestoreProperty]
        public required string Date { get; set; }

        [FirestoreProperty(ConverterType = typeof(FirestoreDecimalConverter))]
        public required decimal Amount { get; set; }

        [FirestoreProperty]
        public string? MerchantName { get; set; }

        [FirestoreProperty]
        public string? Description { get; set; }

        [FirestoreProperty]
        public string? Category { get; set; }

        [FirestoreProperty]
        public required bool IsPending { get; set; }
    }
}
