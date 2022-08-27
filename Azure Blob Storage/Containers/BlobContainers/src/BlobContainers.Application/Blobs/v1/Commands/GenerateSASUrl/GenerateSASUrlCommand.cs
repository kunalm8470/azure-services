using MediatR;

namespace BlobContainers.Application.Blobs.v1.Commands.GenerateSASUrl;

public class GenerateSASUrlCommand : IRequest<SASUrlDto>
{
    public string BlobName { get; set; }
}
