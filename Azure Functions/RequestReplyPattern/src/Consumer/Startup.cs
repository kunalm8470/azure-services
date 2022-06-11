using Azure.Data.Tables;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SharedLibs.Contracts;
using SharedLibs.Services;
using System;

[assembly: FunctionsStartup(typeof(Consumer.Startup))]
namespace Consumer
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton((implementationFactory) =>
            {
                string tableStorageConnectionString = Environment.GetEnvironmentVariable("TableStorageConnectionString");
                string tableName = Environment.GetEnvironmentVariable("TableStorageTableName");

                return new TableClient(tableStorageConnectionString, tableName);
            });

            builder.Services.AddScoped<IJobStatusRepository, JobStatusRepository>();

            builder.Services.AddLogging();
        }
    }
}
