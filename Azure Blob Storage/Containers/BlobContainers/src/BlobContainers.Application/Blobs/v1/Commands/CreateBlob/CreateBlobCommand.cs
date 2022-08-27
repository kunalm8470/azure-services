using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlobContainers.Application.Blobs.v1.Commands.CreateBlob;

public class CreateBlobCommand : IRequest<Unit>
{
    public IFormFile File { get; set; }
}
