using System.Text.Json.Serialization;

namespace SharedLibs.Models
{
    public class ClaimCheck
    {
        [JsonPropertyName("file_name")]
        public string FileName { get; set; }

        [JsonPropertyName("location")]
        public string BlobLocation { get; set; }

        [JsonPropertyName("processing_time")]
        public DateTime ProcessingTime { get; set; }
    }
}
