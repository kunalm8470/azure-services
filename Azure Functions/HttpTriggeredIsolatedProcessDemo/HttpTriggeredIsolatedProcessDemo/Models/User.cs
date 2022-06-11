using System.Text.Json.Serialization;

namespace HttpTriggeredIsolatedProcessDemo.Models
{
    public record User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("username")]
        public string? UserName { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("address")]
        public UserAddress? Address { get; set; }

        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("company")]
        public UserCompany? Company { get; set; }
    }

    public record UserAddress
    {
        [JsonPropertyName("street")]
        public string? Street { get; set; }

        [JsonPropertyName("suite")]
        public string? Suite { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("zipcode")]
        public string? Zipcode { get; set; }

        [JsonPropertyName("geo")]
        public UserAddressCoordinates? Coordinates { get; set; }
    }

    public record UserAddressCoordinates
    {
        [JsonPropertyName("lat")]
        public string? Latitude { get; set; }

        [JsonPropertyName("lng")]
        public string? Longitude { get; set; }
    }

    public record UserCompany
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("catchPhrase")]
        public string? CatchPhrase { get; set; }

        [JsonPropertyName("bs")]
        public string? TrademarkLine { get; set; }
    }
}
