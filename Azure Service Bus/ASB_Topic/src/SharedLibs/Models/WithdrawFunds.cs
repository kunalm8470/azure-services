using System.Text.Json.Serialization;

namespace SharedLibs.Models
{
    public class WithdrawFunds
    {
        [JsonPropertyName("account_id")]
        public int AccountId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }

        [JsonPropertyName("transaction_id")]
        public string TransactionId { get; set; }

        [JsonPropertyName("transaction_date")]
        public DateTime TransactionDate { get; set; }
    }
}
