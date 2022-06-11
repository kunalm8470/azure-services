using Azure;
using Azure.Data.Tables;
using FluentValidation.AspNetCore;
using MediatR;
using System.Reflection;
using TableStorageDemo.API.Middlewares;
using TableStorageDemo.Application.Handlers.Employees.Commands;
using TableStorageDemo.Application.Handlers.Employees.Queries;
using TableStorageDemo.Domain.Contracts;
using TableStorageDemo.Domain.Entities;
using TableStorageDemo.Infrastructure.TableStorage.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton((implementationFactory) =>
{
    string tableStorageConnectionString = builder.Configuration.GetConnectionString("TableStorage");
    string tableName = builder.Configuration.GetValue<string>("TableStorage:employeeTableName");

    return new TableClient(tableStorageConnectionString, tableName);
});

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// Give preference to fluent validation instead of default ASP .NET core validation
builder.Services.AddMvc()
.AddFluentValidation(fv =>
{
    fv.RegisterValidatorsFromAssemblyContaining<Program>();
    fv.DisableDataAnnotationsValidation = true;
});

// Registering MediatR handlers
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddTransient<IRequestHandler<GetEmployeeByPartitionKeyAndRowKeyQuery, Employee?>, GetEmployeeByPartitionKeyAndRowKeyQueryHandler>();
builder.Services.AddTransient<IRequestHandler<CreateEmployeeCommand, Response>, CreateEmployeeCommandHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateEmployeeCommand, Response>, UpdateEmployeeCommandHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteEmployeeCommand, Unit>, DeleteEmployeeCommandHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

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

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseMiddleware<UnhandledExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
