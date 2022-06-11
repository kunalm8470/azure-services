using System.Text.Json.Serialization;

namespace TodoAPI.Models.Requests
{
    public class UpdateTodo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("completed")]
        public bool Completed { get; set; }
    }
}
