using System.Text.Json.Serialization;
using TodoAPI.Domain.Entities;

namespace TodoAPIMongo.Models.Responses
{
    public class PagedTodo
    {
        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("data")]
        public IEnumerable<TodoModel> Data { get; set; }
    }
}
