using Azure.Messaging.ServiceBus;
using SharedLibs.Contracts;
using System.Text.Json;

namespace SharedLibs.Services
{
    public class AzureServiceBusMessagePublisher : IMessagePublisher
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusSender _serviceBusSender;

        public AzureServiceBusMessagePublisher(ServiceBusClient serviceBusClient, ServiceBusSender serviceBusSender)
        {
            _serviceBusClient = serviceBusClient;
            _serviceBusSender = serviceBusSender;
        }

        public async Task PublishMessageAsync<T>(T message, string messageId, string? correlationId = default, string? replyTo = default, CancellationToken cancellationToken = default) where T : class
        {
            if (message == default)
            {
                throw new ArgumentNullException(nameof(message), "Message to publish cannot be empty!");
            }

            string serialized = JsonSerializer.Serialize<T>(message);

            ServiceBusMessage payload = new(serialized);
            payload.MessageId = messageId;

            if (!string.IsNullOrEmpty(correlationId))
            {
                payload.CorrelationId = correlationId;
            }

            payload.ApplicationProperties["messageType"] = typeof(T).Name;
            
            if (!string.IsNullOrEmpty(replyTo))
            {
                payload.ReplyTo = replyTo;
            }

            await _serviceBusSender.SendMessageAsync(payload, cancellationToken);
        }
    }
}
