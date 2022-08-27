using BlobContainers.Application.Common.Configuration;
using BlobContainers.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Options;

namespace BlobContainers.Application.Blobs.v1.Commands.CreateBlob;

public class CreateBlobCommandHandler : IRequestHandler<CreateBlobCommand, Unit>
{
    private readonly AzureBlobStorageConfiguration _configuration;

    private readonly IBlobService _blobService;

    public CreateBlobCommandHandler(IOptions<AzureBlobStorageConfiguration> configuration, IBlobService blobService)
    {
        _configuration = configuration.Value;

        _blobService = blobService;
    }

    public async Task<Unit> Handle(CreateBlobCommand request, CancellationToken cancellationToken)
    {
        Stream readStream = request.File.OpenReadStream();

        string blobName = request.File.FileName;

        await _blobService.UploadBlobAsync(readStream, blobName, _configuration.ContainerName, cancellationToken);

        return Unit.Value;
    }
}
