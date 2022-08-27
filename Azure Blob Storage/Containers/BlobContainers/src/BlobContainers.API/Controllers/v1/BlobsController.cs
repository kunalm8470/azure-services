using Azure.Storage.Blobs.Models;
using BlobContainers.Application.Blobs.v1.Commands.CreateBlob;
using BlobContainers.Application.Blobs.v1.Commands.DeleteBlob;
using BlobContainers.Application.Blobs.v1.Commands.GenerateSASUrl;
using BlobContainers.Application.Blobs.v1.Queries.GetBlobByName;
using BlobContainers.Application.Blobs.v1.Queries.ListBlobs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlobContainers.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BlobsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlobsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<string>>> ListBlobNames(CancellationToken cancellationToken)
        {
            IEnumerable<string> blobNames = await _mediator.Send(new ListBlobsQuery(), cancellationToken);

            return Ok(blobNames);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetBlobByName([FromRoute] GetBlobByNameQuery query, CancellationToken cancellationToken)
        {
            BlobDownloadResult downloadResult = await _mediator.Send(query, cancellationToken);

            if (downloadResult is null)
            {
                return NotFound();
            }

            return File(downloadResult.Content.ToArray(), downloadResult.Details.ContentType);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlob([FromForm(Name = "inputFile")] IFormFile file, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CreateBlobCommand { File = file }, cancellationToken);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("sas")]
        public async Task<ActionResult<SASUrlDto>> GenerateSasUrlForBlobUpload([FromBody] GenerateSASUrlCommand command, CancellationToken cancellationToken)
        {
            SASUrlDto sasUrl = await _mediator.Send(command, cancellationToken);

            return Ok(sasUrl);
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteBlob([FromRoute] DeleteBlobCommand command, CancellationToken cancellationToken)
        {
            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
