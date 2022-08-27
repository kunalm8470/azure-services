using Azure.Storage.Blobs.Models;
using BlobContainers.Application.Blobs.v1.Commands.CreateBlob;
using BlobContainers.Application.Blobs.v1.Commands.DeleteBlob;
using BlobContainers.Application.Blobs.v1.Commands.GenerateSASUrl;
using BlobContainers.Application.Blobs.v1.Queries.GetBlobByName;
using BlobContainers.Application.Blobs.v1.Queries.ListBlobs;
using BlobContainers.Application.Common.Configuration;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlobContainers.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureBlobStorageConfiguration>((a) =>
        {
            a.AccountName = configuration["AzureBlobStorage:accountName"];
            a.AccountKey = configuration["AzureBlobStorage:accountKey"];
            a.ContainerName = configuration["AzureBlobStorage:containerName"];
        });

        // Register FluentValidation
        services
        .AddValidatorsFromAssemblyContaining<GenerateSASUrlCommandValidator>()
        .AddFluentValidationAutoValidation((FluentValidationAutoValidationConfiguration configuration) =>
        {
            configuration.DisableDataAnnotationsValidation = true;
        });

        // Register MediatR
        services.AddMediatR(typeof(CreateBlobCommand));

        //services.AddTransient<IRequestHandler<CreateBlobCommand, Unit>, CreateBlobCommandHandler>();
        //services.AddTransient<IRequestHandler<DeleteBlobCommand, Unit>, DeleteBlobCommandHandler>();
        //services.AddTransient<IRequestHandler<GenerateSASUrlCommand, SASUrlDto>, GenerateSASUrlCommandHandler>();
        //services.AddTransient<IRequestHandler<ListBlobsQuery, IEnumerable<string>>, ListBlobsQueryHandler>();
        //services.AddTransient<IRequestHandler<GetBlobByNameQuery, BlobDownloadResult>, GetBlobByNameQueryHandler>();

        return services;
    }
}
