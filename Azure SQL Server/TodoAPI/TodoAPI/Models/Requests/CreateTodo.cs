using System.Text.Json.Serialization;

namespace TodoAPI.Models.Requests
{
    public class CreateTodo
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
