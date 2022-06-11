using Azure;
using Azure.Data.Tables;

namespace SharedLibs.Models
{
    public class Job : ITableEntity
    {
        public string Id { get; set; }
        public string CorrelationId { get; set; }
        public string Status { get; set; }
        public string StatusUrl { get; set; }
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }

    public static class JobConstants
    {
        public const string PENDING = "pending";
        public const string PROCESSING = "processing";
        public const string FAILED = "failed";
        public const string COMPLETED = "completed";
    }
}
