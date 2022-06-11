using System.Text.Json.Serialization;

namespace TableStorageDemo.API.Models.Requests
{
    public class AddEmployee
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("department")]
        public string Department { get; set; }
    }
}
