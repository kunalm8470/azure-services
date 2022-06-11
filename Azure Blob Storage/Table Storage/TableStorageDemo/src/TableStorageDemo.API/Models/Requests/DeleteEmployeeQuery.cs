using System.Text.Json.Serialization;

namespace TableStorageDemo.API.Models.Requests
{
    public class DeleteEmployeeQuery
    {
        [JsonPropertyName("partition_key")]
        public string PartitionKey { get; set; }

        [JsonPropertyName("row_key")]
        public string RowKey { get; set; }

        [JsonPropertyName("etag")]
        public string Etag { get; set; }
    }
}
