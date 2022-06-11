using Azure.Messaging.ServiceBus;
using SharedLibs;
using SharedLibs.Models;
using System.Text.Json;

string sendConnectionString = "";

string queueName = "sessions_queue";

await using ServiceBusClient client = new ServiceBusClient(sendConnectionString);

ServiceBusSender sender = client.CreateSender(queueName);

Random rnd = new();

async Task PublishMessageAsync<T>(T message) where T : class
{
    if (message == default) throw new ArgumentNullException(nameof(message), "Message to be posted can't be null!");

    string serialized = JsonSerializer.Serialize<T>(message);
    ServiceBusMessage payload = new(serialized);
    payload.MessageId = Guid.NewGuid().ToString("N");
    payload.SessionId = rnd.Next(1, 4).ToString(); // Create custom sessionId here

    await sender.SendMessageAsync(payload).ConfigureAwait(false);
}

Console.WriteLine("Start sending messages");

foreach (Todo item in TestTodos.GenerateTestData())
{
    await PublishMessageAsync(item).ConfigureAwait(false);
}

Console.WriteLine("Finished sending messages");
Console.ReadKey();
