using Azure.Messaging.ServiceBus;
using Consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {

        string listenConnectionString = hostContext.Configuration.GetConnectionString("ServiceBusListenConnectionString");
        string topicName = hostContext.Configuration.GetValue<string>("AzureServiceBus:topicName");
        string depositSubscription = hostContext.Configuration.GetValue<string>("AzureServiceBus:subscription:deposit");
        string withdrawSubscription = hostContext.Configuration.GetValue<string>("AzureServiceBus:subscription:withdraw");
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

        ILoggerFactory loggerFactory = LoggerFactory.Create(logging =>
        {
            logging.Configure(options =>
            {
                options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
                                                    | ActivityTrackingOptions.TraceId
                                                    | ActivityTrackingOptions.ParentId
                                                    | ActivityTrackingOptions.Baggage
                                                    | ActivityTrackingOptions.Tags;
            })
            .AddSimpleConsole(options =>
            {
                options.IncludeScopes = true;
            });
        });

        services.AddHostedService<DepositFundsWorker>((implementationFactory) =>
        {
            ILogger<DepositFundsWorker> logger = loggerFactory.CreateLogger<DepositFundsWorker>();

            ServiceBusProcessor processor = client.CreateProcessor(topicName, depositSubscription, new ServiceBusProcessorOptions
            {
                ReceiveMode = ServiceBusReceiveMode.PeekLock,
                AutoCompleteMessages = false
            });

            return new DepositFundsWorker(logger, processor);
        });

        services.AddHostedService<WithdrawFundsWorker>((implementationFactory) =>
        {
            ILogger<WithdrawFundsWorker> logger = loggerFactory.CreateLogger<WithdrawFundsWorker>();

            ServiceBusProcessor processor = client.CreateProcessor(topicName, withdrawSubscription, new ServiceBusProcessorOptions
            {
                ReceiveMode = ServiceBusReceiveMode.PeekLock,
                AutoCompleteMessages = false
            });

            return new WithdrawFundsWorker(logger, processor);
        });
    })
    .Build();

await host.RunAsync();
