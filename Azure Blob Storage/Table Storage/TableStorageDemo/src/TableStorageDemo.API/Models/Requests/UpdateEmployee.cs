using System.Text.Json.Serialization;

namespace TableStorageDemo.API.Models.Requests
{
    public class UpdateEmployee
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("department")]
        public string Department { get; set; }

        [JsonPropertyName("partition_key")]
        public string PartitionKey { get; set; }

        [JsonPropertyName("row_key")]
        public string RowKey { get; set; }

        [JsonPropertyName("etag")]
        public string Etag { get; set; }
    }
}
