using System.Text.Json.Serialization;

namespace SharedLibs.Models
{
    public record UserComment
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
