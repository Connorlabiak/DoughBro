using System.Text.Json.Serialization;

namespace DoughBro.src.DTOs
{
    public class CreateCategoryRequest
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("colorId")]
        public required string ColorId { get; set; }
    }
}
