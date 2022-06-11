using Api;

var builder = WebApplication.CreateBuilder(args);

string appConfigurationConnectionString = builder.Configuration.GetConnectionString("AzureAppConfig");

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var settings = config.Build();

    config.AddAzureAppConfiguration((options) =>
    {
        options.Connect(appConfigurationConnectionString)
               .ConfigureRefresh((refresh) =>
               {
                   refresh.Register("appconf:sentinelKey", refreshAll: true)
                          .SetCacheExpiration(new TimeSpan(0, 0, 5));
               });
    });
});

builder.Services.Configure<AzureAppConfigurationDto>(
    builder.Configuration.GetSection("appconf")
);

// Add services to the container.

builder.Services.AddControllers();

// Add Azure App configuration components to the IServiceCollection
builder.Services.AddAzureAppConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Add UseAzureAppConfiguration middleware to monitor for updates the keys
app.UseAzureAppConfiguration();

app.UseAuthorization();

app.MapControllers();

app.Run();
