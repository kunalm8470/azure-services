using System.Text.Json.Serialization;

namespace TableStorageDemo.API.Models.Requests
{
    public class GetEmployeeQuery
    {
        [JsonPropertyName("partition_key")]
        public string PartitionKey { get; set; }

        [JsonPropertyName("row_key")]
        public string RowKey { get; set; }
    }
}
