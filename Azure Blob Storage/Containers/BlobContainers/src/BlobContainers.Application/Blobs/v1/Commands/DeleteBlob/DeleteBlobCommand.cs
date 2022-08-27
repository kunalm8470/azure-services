using MediatR;

namespace BlobContainers.Application.Blobs.v1.Commands.DeleteBlob;

public class DeleteBlobCommand : IRequest<Unit>
{
    public string BlobName { get; set; }
}
