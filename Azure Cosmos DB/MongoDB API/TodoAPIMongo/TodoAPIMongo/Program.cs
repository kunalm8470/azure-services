using FluentValidation.AspNetCore;
using MediatR;
using System.Reflection;
using TodoAPI.Application.Handlers.Todos.Commands;
using TodoAPI.Application.Handlers.Todos.Queries;
using TodoAPI.Domain.Contracts;
using TodoAPI.Domain.Entities;
using TodoAPI.Infrastructure;
using TodoAPI.Infrastructure.Database.Repositories;
using TodoAPIMongo.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add console logging
builder.Logging.AddConsole();

// Register MongoDbConnectionFactory
builder.Services.AddSingleton((implementationFactory) =>
{
    string connectionString = builder.Configuration.GetConnectionString("Default");
    string databaseName = builder.Configuration["MongoDb:DatabaseName"];

    return new MongoDbConnectionFactory(connectionString, databaseName);
});

// Register todo repository
builder.Services.AddScoped<ITodoRepository, TodoRepository>((implementationFactory) =>
{
    var connectionFactory = implementationFactory.GetService<MongoDbConnectionFactory>();
    string collectionName = builder.Configuration["MongoDb:CollectionName"];
    return new TodoRepository(connectionFactory, collectionName);
});

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
builder.Services.AddTransient<IRequestHandler<GetTodoByIdQuery, Todo?>, GetTodoByIdQueryHandler>();
builder.Services.AddTransient<IRequestHandler<CreateTodoCommand, Unit>, CreateTodoCommandHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateTodoCommand, Todo>, UpdateTodoCommandHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteTodoCommand, Unit>, DeleteTodoCommandHandler>();


builder.Services.AddControllers();

// Add allow all CORS policy
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
