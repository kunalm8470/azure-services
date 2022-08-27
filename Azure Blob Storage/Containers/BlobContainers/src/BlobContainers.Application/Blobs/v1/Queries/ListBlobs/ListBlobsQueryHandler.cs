using BlobContainers.Application.Common.Configuration;
using BlobContainers.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Options;

namespace BlobContainers.Application.Blobs.v1.Queries.ListBlobs;

public class ListBlobsQueryHandler : IRequestHandler<ListBlobsQuery, IEnumerable<string>>
{
    private readonly AzureBlobStorageConfiguration _configuration;

    private readonly IBlobService _blobService;

    public ListBlobsQueryHandler(IOptions<AzureBlobStorageConfiguration> configuration, IBlobService blobService)
    {
        _configuration = configuration.Value;

        _blobService = blobService;
    }
    
    public async Task<IEnumerable<string>> Handle(ListBlobsQuery request, CancellationToken cancellationToken)
    {
        return await _blobService.ListBlobsAsync(_configuration.ContainerName, cancellationToken);
    }
}
