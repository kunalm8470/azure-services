using Azure.Storage;
using Azure.Storage.Blobs;
using BlobContainers.Domain.Interfaces.Services;
using BlobContainers.Infrastructure.ThirdPartyServices.AzureBlobStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlobContainers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IBlobService>((serviceProvider) =>
        {
            string accountName = configuration["AzureBlobStorage:accountName"];
            
            string accountKey = configuration["AzureBlobStorage:accountKey"];

            Uri serviceUri = new($"https://{accountName}.blob.core.windows.net");

            StorageSharedKeyCredential storageSharedKeyCredential = new(accountName, accountKey);

            BlobServiceClient client = new(serviceUri, storageSharedKeyCredential);

            return new BlobService(client);
        });

        return services;
    }
}
