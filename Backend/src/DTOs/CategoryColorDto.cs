using System.Text.Json.Serialization;

namespace DoughBro.src.DTOs
{
    public class CategoryColorDto
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("hex")]
        public required string Hex { get; set; }

        [JsonPropertyName("isUsed")]
        public required bool IsUsed { get; set; }
    }
}
