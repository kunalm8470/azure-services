using Azure.Messaging.ServiceBus;
using Producer.Contracts;
using System.Text.Json;

namespace Producer.Services
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

        public async Task PublishMessageAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            if (message == default)
            {
                throw new ArgumentNullException(nameof(message), "Message to publish cannot be empty!");
            }

            string serialized = JsonSerializer.Serialize<T>(message);

            ServiceBusMessage payload = new(serialized);
            payload.MessageId = Guid.NewGuid().ToString("N");
            payload.Subject = "Todo message";
            payload.ApplicationProperties["messageType"] = typeof(T).Name;

            await _serviceBusSender.SendMessageAsync(payload, cancellationToken);
        }
    }
}
