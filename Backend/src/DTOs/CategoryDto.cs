using System.Text.Json.Serialization;

namespace DoughBro.src.DTOs
{
    public class CategoryDto
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonRequired]
        [JsonPropertyName("name")]
        public required string Name { get; set; }

    }
}
