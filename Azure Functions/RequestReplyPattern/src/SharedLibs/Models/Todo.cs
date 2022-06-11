using System.Text.Json.Serialization;

namespace SharedLibs.Models
{
    public record Todo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
    }
}
