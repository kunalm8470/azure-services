using BlobContainers.Application.Common.Configuration;
using BlobContainers.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Options;

namespace BlobContainers.Application.Blobs.v1.Commands.DeleteBlob;

public class DeleteBlobCommandHandler : IRequestHandler<DeleteBlobCommand, Unit>
{
	private readonly AzureBlobStorageConfiguration _configuration;

	private readonly IBlobService _blobService;

	public DeleteBlobCommandHandler(IOptions<AzureBlobStorageConfiguration> configuration, IBlobService blobService)
	{
		_configuration = configuration.Value;

		_blobService = blobService;
	}

	public async Task<Unit> Handle(DeleteBlobCommand request, CancellationToken cancellationToken)
	{
		await _blobService.DeleteBlobAsync(_configuration.ContainerName, request.BlobName, cancellationToken);

		return Unit.Value;
	}
}
