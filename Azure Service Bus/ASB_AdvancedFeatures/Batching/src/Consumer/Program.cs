using Azure.Messaging.ServiceBus;
using Consumer;

IHost host = Host.CreateDefaultBuilder(args)
     .ConfigureServices((hostContext, services) =>
     {
         string listenConnectionString = hostContext.Configuration.GetConnectionString("ServiceBusListenConnectionString");

         string queueName = hostContext.Configuration.GetValue<string>("ServiceBus:queueName");

         services.AddSingleton<ServiceBusProcessor>((implementationFactory) =>
         {
             ServiceBusClient client = new(listenConnectionString);

             return client.CreateProcessor(queueName);
         });

         services.AddHostedService<Worker>();
     })
    .Build();

await host.RunAsync();
