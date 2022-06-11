using FluentValidation.AspNetCore;
using MediatR;
using StackExchange.Redis;
using System.Reflection;
using TodoAPI.Application.Handlers.Todos.Commands;
using TodoAPI.Application.Handlers.Todos.Queries;
using TodoAPI.Domain.Contracts;
using TodoAPI.Domain.Entities;
using TodoAPI.Infrastructure.Database.Repositories;
using TodoAPIRedis.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add console logging
builder.Logging.AddConsole();

// Register ConnectionMultiplexer as Singleton
// because ConnectionMultiplexer is to be shared and reused.
// See https://stackexchange.github.io/StackExchange.Redis/Basics.html#basic-usage
builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
{
    string redisConnectionString = builder.Configuration.GetConnectionString("Redis");

    if (string.IsNullOrWhiteSpace(redisConnectionString))
    {
        throw new ArgumentNullException(nameof(redisConnectionString), "Invalid redis connection string");
    }

    // For full options see https://stackexchange.github.io/StackExchange.Redis/Configuration.html
    ConfigurationOptions options = ConfigurationOptions.Parse(redisConnectionString);

    options.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
    options.ConnectRetry = builder.Configuration.GetValue<int>("Redis:maxRetry");

    // See https://stackexchange.github.io/StackExchange.Redis/Configuration.html#reconnectretrypolicy
    options.ReconnectRetryPolicy = new LinearRetry(builder.Configuration.GetValue<int>("Redis:retryReconnectMilliSecs"));

    ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(options);

    return connectionMultiplexer;
});

builder.Services.AddControllers();

// Register todo repository
builder.Services.AddTransient<ITodoRepository, TodoRepository>();

// Give preference to fluent validation instead of default ASP .NET core validation
builder.Services.AddMvc()
.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<Program>();
    fv.DisableDataAnnotationsValidation = true;
});

// Registering MediatR handlers
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddTransient<IRequestHandler<ListTodosQuery, (IEnumerable<Todo>, long)>, ListTodosQueryHandler>();
builder.Services.AddTransient<IRequestHandler<GetTodoByIdQuery, Todo>, GetTodoByIdQueryHandler>();
builder.Services.AddTransient<IRequestHandler<CreateTodoCommand, Unit>, CreateTodoCommandHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateTodoCommand, Unit>, UpdateTodoCommandHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteTodoCommand, Unit>, DeleteTodoCommandHandler>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(o =>
    {
        o.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<UnhandledExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
