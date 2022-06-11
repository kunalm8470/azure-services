using System.Text.Json.Serialization;

namespace Producer.Models.Requests
{
    public record TodoDto
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
