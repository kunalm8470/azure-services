using Azure.Messaging.ServiceBus;
using Consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        string listenConnectionString = hostContext.Configuration.GetConnectionString("ServiceBusListenConnectionString");

        string queueName = hostContext.Configuration.GetValue<string>("ServiceBus:queueName");

        services.AddSingleton<ServiceBusSessionProcessor>((implementationFactory) =>
        {
            ServiceBusClient client = new(listenConnectionString);

            return client.CreateSessionProcessor(queueName);
        });

        services.AddHostedService<SessionMessagesWorker>();
    })
    .Build();

await host.RunAsync();
