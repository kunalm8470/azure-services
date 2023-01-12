using Azure.Storage.Queues;
using Producer.Middlewares;
using SharedLibs.Contracts;
using SharedLibs.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IStorageQueueService, StorageQueueService>((implementationFactory) =>
{
    string connectionString = builder.Configuration.GetConnectionString("AzureStorageQueue");
    string queueName = builder.Configuration.GetValue<string>("StorageQueue:name");
    string poisonQueueName = builder.Configuration.GetValue<string>("StorageQueue:poisonQueueName");
    int delaySecs = builder.Configuration.GetValue<int>("StorageQueue:delay");
    int maxRetries = builder.Configuration.GetValue<int>("StorageQueue:maxRetries");

    QueueClientOptions options = new();
    options.Retry.Delay = TimeSpan.FromSeconds(delaySecs);
    options.Retry.MaxRetries = maxRetries;
    options.Retry.Mode = Azure.Core.RetryMode.Fixed;

    QueueClient client = new(connectionString, queueName, options);
    QueueClient poisonQueueClient = new(connectionString, poisonQueueName, options);

    return new StorageQueueService(client, poisonQueueClient);
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(o =>
    {
        o.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
        );
    });
}

builder.Services.AddControllers();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<UnhandledExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
