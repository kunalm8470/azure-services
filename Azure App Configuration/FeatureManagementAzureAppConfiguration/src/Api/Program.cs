using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

string appConfigurationConnectionString = builder.Configuration.GetConnectionString("AzureAppConfig");

// Connect to Azure App configuration
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.AddAzureAppConfiguration((options) =>
    {
        options.Connect(appConfigurationConnectionString);

        options.UseFeatureFlags((featureFlagOptions) =>
        {
            // Reload feature flags every 5 seconds
            featureFlagOptions.CacheExpirationInterval = new TimeSpan(0, 0, 5);
        });
    });
});

// Add services to the container.

builder.Services.AddControllers();

// Add Azure App configuration feature management components to the IServiceCollection
builder.Services.AddAzureAppConfiguration();
builder.Services.AddFeatureManagement();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Add UseAzureAppConfiguration middleware to monitor for updates the keys
app.UseAzureAppConfiguration();

app.UseAuthorization();

app.MapControllers();

app.Run();
