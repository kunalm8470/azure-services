using Newtonsoft.Json;

namespace SharedLibs.Models
{
    public class Employee
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }
    }
}
