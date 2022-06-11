using Azure.Storage.Queues;
using Consumer;
using SharedLibs.Contracts;
using SharedLibs.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<IStorageQueueService, StorageQueueService>((implementationFactory) =>
        {
            string connectionString = hostContext.Configuration.GetConnectionString("AzureStorageQueue");
            string queueName = hostContext.Configuration.GetValue<string>("StorageQueue:name");
            string poisonQueueName = hostContext.Configuration.GetValue<string>("StorageQueue:poisonQueueName");
            int delaySecs = hostContext.Configuration.GetValue<int>("StorageQueue:delay");
            int maxRetries = hostContext.Configuration.GetValue<int>("StorageQueue:maxRetries");

            QueueClientOptions options = new();
            options.Retry.Delay = TimeSpan.FromSeconds(delaySecs);
            options.Retry.MaxRetries = maxRetries;
            options.Retry.Mode = Azure.Core.RetryMode.Fixed;

            QueueClient client = new(connectionString, queueName, options);
            QueueClient poisonQueueClient = new(connectionString, poisonQueueName, options);

            return new StorageQueueService(client, poisonQueueClient);
        });

        services.AddHostedService<StorageQueueWorker>();
    })
    .Build();

await host.RunAsync();
