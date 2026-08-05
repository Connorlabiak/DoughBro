using DoughBro.src.Database;
using Google.Cloud.Firestore;
using System.Text.Json.Serialization;

namespace DoughBro.src.DTOs
{
    public class TransactionDto
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("origin")]
        public required string Origin { get; set; }

        [JsonPropertyName("userId")]
        public required string UserId { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("date")]
        public required string Date { get; set; }

        [JsonPropertyName("amount")]
        public required decimal Amount { get; set; }

        [JsonPropertyName("merchantName")]
        public string? MerchantName { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("isPending")]
        public required bool IsPending { get; set; }
    }
}
