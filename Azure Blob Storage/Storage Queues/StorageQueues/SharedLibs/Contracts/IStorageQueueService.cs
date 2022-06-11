using Azure;
using Azure.Storage.Queues.Models;

namespace SharedLibs.Contracts
{
    public interface IStorageQueueService
    {
        Task<Response<SendReceipt>> PublishMessageAsync<T>(T message, TimeSpan? visibilityTimeout = default, TimeSpan? timeToLive = default, CancellationToken cancellationToken = default) where T : class;
        Task<Response<PeekedMessage[]>> PeekMessagesAsync(int maxMessages, CancellationToken cancellationToken = default);
        Task<Response<QueueMessage>> ReceiveMessageAsync(TimeSpan? visibilityTimeout = default, CancellationToken cancellationToken = default);
        Task<Response<UpdateReceipt>> UpdateMessageInPlaceAsync(string messageId, string popReceipt, CancellationToken cancellationToken = default);
        Task<Response> DeleteMessageAsync(string messageId, string popReceipt, CancellationToken cancellationToken = default);
        Task<Response<SendReceipt>> DeadLetterMessageAsync(BinaryData message, TimeSpan? visibilityTimeout = null, TimeSpan? timeToLive = null, CancellationToken cancellationToken = default);
    }
}
