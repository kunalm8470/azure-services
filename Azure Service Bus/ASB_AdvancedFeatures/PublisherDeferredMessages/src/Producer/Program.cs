using Azure.Messaging.ServiceBus;
using SharedLibs;
using SharedLibs.Models;
using System.Text.Json;

string sendConnectionString = "";

string queueName = "publisher_message_deferral_queue";

await using ServiceBusClient client = new ServiceBusClient(sendConnectionString);

ServiceBusSender sender = client.CreateSender(queueName);

Random rnd = new();

async Task PublishMessageAsync<T>(T message) where T : class
{
    if (message == default) throw new ArgumentNullException(nameof(message), "Message to be posted can't be null!");

    string serialized = JsonSerializer.Serialize<T>(message);
    ServiceBusMessage payload = new(serialized);
    payload.MessageId = Guid.NewGuid().ToString("N");

    payload.ScheduledEnqueueTime = DateTimeOffset.Now.Add(new TimeSpan(0, 0, 30));

    await sender.SendMessageAsync(payload).ConfigureAwait(false);
}

Console.WriteLine("Start sending message");

foreach (Todo item in TestTodos.GenerateTestData().Take(1))
{
    await PublishMessageAsync(item).ConfigureAwait(false);
}

Console.WriteLine("Finished sending message");
Console.ReadKey();
