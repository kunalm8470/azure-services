using Azure.Storage.Blobs.Models;
using BlobContainers.Application.Common.Configuration;
using BlobContainers.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Options;

namespace BlobContainers.Application.Blobs.v1.Queries.GetBlobByName;

public class GetBlobByNameQueryHandler : IRequestHandler<GetBlobByNameQuery, BlobDownloadResult>
{
    private readonly AzureBlobStorageConfiguration _configuration;

    private readonly IBlobService _blobService;

    public GetBlobByNameQueryHandler(IOptions<AzureBlobStorageConfiguration> configuration, IBlobService blobService)
    {
        _configuration = configuration.Value;

        _blobService = blobService;
    }

    public async Task<BlobDownloadResult> Handle(GetBlobByNameQuery request, CancellationToken cancellationToken)
    {
        return await _blobService.GetBlobAsync(_configuration.ContainerName, request.Name, cancellationToken);
    }
}
