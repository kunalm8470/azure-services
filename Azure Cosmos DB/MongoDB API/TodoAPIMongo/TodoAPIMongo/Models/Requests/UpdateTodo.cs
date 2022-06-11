using System.Text.Json.Serialization;

namespace TodoAPIMongo.Models.Requests
{
    public class UpdateTodo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
    }
}
