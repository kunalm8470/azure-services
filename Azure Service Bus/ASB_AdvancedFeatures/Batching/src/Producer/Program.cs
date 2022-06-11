using Azure.Messaging.ServiceBus;
using SharedLibs;
using SharedLibs.Models;
using System.Text.Json;

string sendConnectionString = "";

string queueName = "batching_queue";

await using ServiceBusClient client = new(sendConnectionString);

ServiceBusSender sender = client.CreateSender(queueName);

ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync(new CreateMessageBatchOptions
{
    MaxSizeInBytes = default(long?)
}).ConfigureAwait(false);

async Task PublishMessagesBatchAsync(ServiceBusMessageBatch batchMessages)
{
    await sender.SendMessagesAsync(batchMessages).ConfigureAwait(false);
}

bool TryAddBatchMessage<T>(T message) where T : class
{
    if (message == default) throw new ArgumentNullException(nameof(message), "Message to be posted can't be null!");

    string serialized = JsonSerializer.Serialize<T>(message);
    ServiceBusMessage payload = new(serialized);
    payload.MessageId = Guid.NewGuid().ToString("N");

    return batch.TryAddMessage(payload);
}

Console.WriteLine("Start sending message");

foreach (Todo item in TestTodos.GenerateTestData())
{
    // Try to add message that doesn't exceed
    // 256 KB (Basic/Standard) or 1 MB (Premium) limit
    if (!TryAddBatchMessage(item))
    {
        break;
    }
}

await PublishMessagesBatchAsync(batch).ConfigureAwait(false);

Console.WriteLine("Finished sending message");
Console.ReadKey();
