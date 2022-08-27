using BlobContainers.Application.Common.Configuration;
using BlobContainers.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Options;

namespace BlobContainers.Application.Blobs.v1.Commands.GenerateSASUrl;

public class GenerateSASUrlCommandHandler : IRequestHandler<GenerateSASUrlCommand, SASUrlDto>
{
    private readonly AzureBlobStorageConfiguration _configuration;

    private readonly IBlobService _blobService;

    public GenerateSASUrlCommandHandler(IOptions<AzureBlobStorageConfiguration> configuration, IBlobService blobService)
    {
        _configuration = configuration.Value;

        _blobService = blobService;
    }

    public Task<SASUrlDto> Handle(GenerateSASUrlCommand request, CancellationToken cancellationToken)
    {
        Uri sasUri = _blobService.GenerateSASUrlAsync(
            _configuration.AccountName, 
            _configuration.AccountKey, 
            _configuration.ContainerName, 
            request.BlobName);

        return Task.FromResult(new SASUrlDto
        {
            Url = sasUri.AbsoluteUri
        });
    }
}
