using Google.Cloud.Firestore;

namespace DoughBro.src.Models
{
    [FirestoreData]
    public record PlaidAccessTokenModel
    {
        [FirestoreDocumentId]
        public required string ItemId { get; set; }

        [FirestoreProperty]
        public required string Token { get; set; }

        [FirestoreProperty]
        public required string UserId { get; set; }

        [FirestoreProperty]
        public required string InstitutionName { get; set; }

        [FirestoreProperty]
        public required string CreatedAt { get; set; }

        [FirestoreProperty]
        public string? NextCursor { get; set; }
    }
}
