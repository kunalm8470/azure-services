using FluentValidation.AspNetCore;
using MediatR;
using System.Reflection;
using TodoAPI.Application.Handlers.Commands;
using TodoAPI.Application.Handlers.Queries;
using TodoAPI.Domain.Contracts;
using TodoAPI.Domain.Entities;
using TodoAPI.Infrastructure.Database;
using TodoAPI.Infrastructure.Database.Repositories;
using TodoAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add console logging
builder.Logging.AddConsole();

// Register SqlServerConnectionFactory
builder.Services.AddTransient(implementationFactory =>
{
    string connectionString = builder.Configuration.GetConnectionString("Default");
    return new SqlServerConnectionFactory(connectionString);
});

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

builder.Services.AddTransient<IRequestHandler<ListTodosQuery, (IEnumerable<Todo>, int)>, ListTodosQueryHandler>();
builder.Services.AddTransient<IRequestHandler<GetTodoByIdQuery, Todo?>, GetTodoByIdQueryHandler>();
builder.Services.AddTransient<IRequestHandler<CreateTodoCommand, Unit>, CreateTodoCommandHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateTodoCommand, Unit>, UpdateTodoCommandHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteTodoCommand, Unit>, DeleteTodoCommandHandler>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<UnhandledExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
