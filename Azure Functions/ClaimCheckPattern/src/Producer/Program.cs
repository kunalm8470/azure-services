using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http.Features;
using Producer.Middlewares;
using SharedLibs.Contracts;
using SharedLibs.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IMessagePublisher, AzureServiceBusMessagePublisher>((implementationFactory) =>
{
    string sendConnectionString = builder.Configuration.GetConnectionString("ServiceBusSendConnectionString");
    string queueName = builder.Configuration.GetValue<string>("AzureServiceBus:queueName");
    int retryDelaySecs = builder.Configuration.GetValue<int>("AzureServiceBus:delaySecs");
    int maxRetries = builder.Configuration.GetValue<int>("AzureServiceBus:maxRetries");

    ServiceBusClient client = new(sendConnectionString, new ServiceBusClientOptions
    {
        RetryOptions = new ServiceBusRetryOptions
        {
            Mode = ServiceBusRetryMode.Fixed,
            Delay = TimeSpan.FromSeconds(retryDelaySecs),
            MaxRetries = maxRetries
        }
    });

    ServiceBusSender sender = client.CreateSender(queueName);
    return new AzureServiceBusMessagePublisher(client, sender);
});

builder.Services.AddSingleton<IBlobService, BlobService>((implementationFactory) =>
{
    string connectionString = builder.Configuration.GetConnectionString("BlobStorage");
    BlobServiceClient blobClient = new(connectionString);
    return new BlobService(blobClient);
});

builder.Services.Configure<FormOptions>(options =>
{
    // Set the file size limit to 5 MB
    options.MultipartBodyLengthLimit = 5L * 1024L * 1024L;
});

// Access HTTP context using DI
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<UnhandledExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
