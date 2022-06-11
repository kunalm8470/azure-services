using HostedServiceExample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHostedService<ExampleHostedService>();

builder.Services.AddControllers();

builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
