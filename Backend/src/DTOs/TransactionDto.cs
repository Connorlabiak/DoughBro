using System.Text.Json.Serialization;

namespace DoughBro.src.DTOs
{
    public class TransactionDto
    {
        [JsonPropertyName("id")]
        public string? UserId { get; set; }

        [JsonRequired]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonRequired]
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonRequired]
        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("category")]
        public string? category { get; set; }
    }
}
