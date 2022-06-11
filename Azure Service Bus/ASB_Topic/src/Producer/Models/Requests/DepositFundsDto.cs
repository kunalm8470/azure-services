using System.Text.Json.Serialization;

namespace Producer.Models.Requests
{
    public class DepositFundsDto
    {
        [JsonPropertyName("account_id")]
        public int AccountId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }
    }
}
