using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using SharedLibs.Contracts;
using System.Text.Json;

namespace SharedLibs.Services
{
    public class StorageQueueService : IStorageQueueService
    {
        private readonly QueueClient _client;
        private readonly QueueClient _poisonQueueClient;

        public StorageQueueService(QueueClient client, QueueClient poisonQueueClient)
        {
            _client = client;
            _poisonQueueClient = poisonQueueClient;
        }

        public async Task<Response<SendReceipt>> DeadLetterMessageAsync(BinaryData message, TimeSpan? visibilityTimeout = null, TimeSpan? timeToLive = null, CancellationToken cancellationToken = default)
        {   
            return await _poisonQueueClient.SendMessageAsync(message, visibilityTimeout, timeToLive, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Response> DeleteMessageAsync(string messageId, string popReceipt, CancellationToken cancellationToken = default)
        {
            return await _client.DeleteMessageAsync(messageId, popReceipt, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Response<PeekedMessage[]>> PeekMessagesAsync(int maxMessages, CancellationToken cancellationToken = default)
        {
            return await _client.PeekMessagesAsync(maxMessages, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Response<SendReceipt>> PublishMessageAsync<T>(T message, TimeSpan? visibilityTimeout = default, TimeSpan? timeToLive = default, CancellationToken cancellationToken = default) where T : class
        {
            if (message == default)
            {
                throw new ArgumentNullException(nameof(message), "Message to publish cannot be empty!");
            }

            string serialized = JsonSerializer.Serialize<T>(message);
            return await _client.SendMessageAsync(serialized, visibilityTimeout, timeToLive, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Response<QueueMessage>> ReceiveMessageAsync(TimeSpan? visibilityTimeout = default, CancellationToken cancellationToken = default)
        {
            return await _client.ReceiveMessageAsync(visibilityTimeout, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Response<UpdateReceipt>> UpdateMessageInPlaceAsync(string messageId, string popReceipt, CancellationToken cancellationToken = default)
        {
            return await _client.UpdateMessageAsync(messageId, popReceipt).ConfigureAwait(false);
        }
    }
}
