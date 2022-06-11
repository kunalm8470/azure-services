using Azure.Storage.Blobs;
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
            builder.Services.AddSingleton<IBlobService, BlobService>((implementationFactory) =>
            {
                string connectionString = Environment.GetEnvironmentVariable("BlobStorageConnectionString");
                BlobServiceClient blobClient = new(connectionString);
                return new BlobService(blobClient);
            });

            builder.Services.AddLogging();
        }
    }
}
