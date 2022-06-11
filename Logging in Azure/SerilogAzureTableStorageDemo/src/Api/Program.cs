using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string tableStorageConnectionString = builder.Configuration.GetConnectionString("TableStorageConnectionString");
string tableName = builder.Configuration.GetValue<string>("TableStorage:tableName");

builder.Host.UseSerilog((HostBuilderContext ctx, LoggerConfiguration lc) => 
    lc
    .Enrich.WithProcessId() // Enrich with process id
    .Enrich.WithThreadId() // Enrich with thread id
    .Enrich.WithMachineName() // Enrich with machine fully qualified name
    .Enrich.WithEnvironmentName() // Enrich with environment name
    .Enrich.WithEnvironmentUserName() // Enrich with Azure AD username
    .MinimumLevel.Information()
    .WriteTo.AzureTableStorage(
        connectionString: tableStorageConnectionString,
        storageTableName: tableName,
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