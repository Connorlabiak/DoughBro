using System.Text.Json.Serialization;

namespace DoughBro.src.DTOs
{
    public class CategoryDto
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("color")]
        public required string Color { get; set; }
    }
}
