using Azure.Storage.Blobs.Models;
using BlobAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace BlobAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBlobService _blobService;

        public BlobsController(IConfiguration configuration, IBlobService blobService)
        {
            _configuration = configuration;
            _blobService = blobService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlobByName([FromQuery] string name)
        {
            string containerName = _configuration["BlobStorage:ContainerName"];
            BlobDownloadResult downloadResult = await _blobService.GetBlobAsync(containerName, name).ConfigureAwait(false);

            if (downloadResult == default)
            {
                return NotFound();
            }

            return File(downloadResult.Content.ToArray(), downloadResult.Details.ContentType);
        }

        [HttpGet("list")]
        public async Task<ActionResult<string[]>> ListBlobNames()
        {
            string containerName = _configuration["BlobStorage:ContainerName"];
            return Ok((await _blobService.ListBlobsAsync(containerName).ConfigureAwait(false)).ToArray());
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlob([FromForm(Name = "myfile")] IFormFile file)
        {
            string containerName = _configuration["BlobStorage:ContainerName"];
            await _blobService.UploadBlobAsync(file, containerName).ConfigureAwait(false);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBlob([FromQuery] string blobName)
        {
            string containerName = _configuration["BlobStorage:ContainerName"];

            await _blobService.DeleteBlobAsync(containerName, blobName).ConfigureAwait(false);

            return NoContent();
        }
    }
}
