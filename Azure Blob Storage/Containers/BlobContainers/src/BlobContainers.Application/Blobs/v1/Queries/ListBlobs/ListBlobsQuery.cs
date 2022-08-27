using MediatR;

namespace BlobContainers.Application.Blobs.v1.Queries.ListBlobs;

public class ListBlobsQuery : IRequest<IEnumerable<string>>
{
}
