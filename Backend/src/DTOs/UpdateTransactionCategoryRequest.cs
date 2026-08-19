using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DoughBro.src.DTOs
{
    public class UpdateTransactionCategoryRequest
    {
        [Required]
        [StringLength(250)]
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [Required]
        [JsonPropertyName("date")]
        public required string Date { get; set; }

        [Range(typeof(decimal), "-999999999.99", "999999999.99")]
        [JsonPropertyName("amount")]
        public required decimal Amount { get; set; }

        [StringLength(250)]
        [JsonPropertyName("merchantName")]
        public string? MerchantName { get; set; }

        [StringLength(1000)]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [Required]
        [StringLength(250)]
        [JsonPropertyName("category")]
        public required string Category { get; set; }
    }
}
