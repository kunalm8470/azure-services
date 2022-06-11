using Serilog;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string blobStorageConnectionString = builder.Configuration.GetConnectionString("BlobStorageConnectionString");
string containerName = builder.Configuration.GetValue<string>("BlobStorage:containerName");

builder.Host.UseSerilog((HostBuilderContext ctx, LoggerConfiguration lc) =>
    lc
    .MinimumLevel.Information()
    .WriteTo.AzureBlobStorage(
        connectionString: blobStorageConnectionString,
        storageContainerName: containerName,
        blobSizeLimitBytes: 5 * 1024 * 1024, // Limit log files to 5 MB in size, it will create new file after exceeding this limit
        storageFileName: $"{{yyyy}}/{{MM}}/{{dd}}/{containerName}.log",
        writeInBatches: true, // Flush logs in batches instead of one by one
        period: TimeSpan.FromSeconds(15), // Post every batch 15 seconds
        batchPostingLimit: 10 // Post if 10 logs or higher
    )
);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
