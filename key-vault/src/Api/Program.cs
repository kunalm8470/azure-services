using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault(
    new Uri(builder.Configuration.GetValue<string>("VaultUri")),
    new DefaultAzureCredential(), 
    new AzureKeyVaultConfigurationOptions
    {
        ReloadInterval = TimeSpan.FromMinutes(10)
    });

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
