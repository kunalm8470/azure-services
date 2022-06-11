using Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient<JsonTypicodeService>((client) =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
});

// Add application insights into the IServiceCollection DI container
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
