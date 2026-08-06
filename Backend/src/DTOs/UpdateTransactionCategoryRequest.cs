using System.Text.Json.Serialization;

namespace DoughBro.src.DTOs
{
    public class UpdateTransactionCategoryRequest
    {
        [JsonPropertyName("category")]
        public required string Category { get; set; }
    }
}
