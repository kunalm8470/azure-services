using Azure.Storage.Blobs.Models;
using MediatR;

namespace BlobContainers.Application.Blobs.v1.Queries.GetBlobByName;

public class GetBlobByNameQuery : IRequest<BlobDownloadResult>
{
    public string Name { get; set; }
}
