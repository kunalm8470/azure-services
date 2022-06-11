using System.Text.Json.Serialization;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Models.Responses
{
    public class PagedTodo
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("data")]
        public IEnumerable<Todo> Data { get; set; }
    }
}
