using System.Text.Json.Serialization;

namespace TodoAPI.Models.Requests
{
    public class ListTodo
    {
        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;

        [JsonPropertyName("limit")]
        public int Limit { get; set; } = 10;
    }
}
