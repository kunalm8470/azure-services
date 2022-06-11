using Azure.Messaging.ServiceBus;
using Consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<ServiceBusProcessor>((implementationFactory) =>
        {
            string listenConnectionString = hostContext.Configuration.GetConnectionString("ServiceBusListenConnectionString");
            string queueName = hostContext.Configuration.GetValue<string>("AzureServiceBus:queueName");
            int retryDelaySecs = hostContext.Configuration.GetValue<int>("AzureServiceBus:delaySecs");
            int maxRetries = hostContext.Configuration.GetValue<int>("AzureServiceBus:maxRetries");

            ServiceBusClient client = new(listenConnectionString, new ServiceBusClientOptions
            {
                RetryOptions = new ServiceBusRetryOptions
                {
                    Mode = ServiceBusRetryMode.Fixed,
                    Delay = TimeSpan.FromSeconds(retryDelaySecs),
                    MaxRetries = maxRetries
                }
            });

            ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions
            {
                ReceiveMode = ServiceBusReceiveMode.PeekLock,
                AutoCompleteMessages = false
            });

            return processor;
        });

        services.AddHostedService<ServiceBusWorker>();
    })
    .Build();

await host.RunAsync();
