using System.Text.Json.Serialization;

namespace SharedLibs.Models
{
    public record UserPost
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("comments")]
        public IReadOnlyList<UserComment> Comments { get; set; }
    }
}
