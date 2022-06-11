using Microsoft.AspNetCore.Mvc;
using SharedLibs.Contracts;
using SharedLibs.Models;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly HttpContext _httpContext;
        private readonly ILogger<EmployeesController> _logger;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IBlobService _blobService;
        private readonly IConfiguration _configuration;

        public EmployeesController(
            IHttpContextAccessor httpContextAccessor,
            IMessagePublisher messagePublisher,
            IBlobService blobService,
            ILogger<EmployeesController> logger,
            IConfiguration configuration
        )
        {
            _messagePublisher = messagePublisher;
            _blobService = blobService;
            _httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor), "HTTP context can't be null");
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("Process")]
        public async Task<ActionResult<ClaimCheck>> BulkUploadAsync([FromForm(Name = "employees")] IFormFile file, CancellationToken cancellationToken)
        {
            string requestId = Guid.NewGuid().ToString();
            string correlationId = _httpContext.Request.Headers["x-correlation-id"];
            string containerName = _configuration["BlobStorage:ContainerName"];

            string fileName = $"{correlationId}_{file.FileName}";

            await using Stream? readStream = file.OpenReadStream();
            
            // Upload the file to Azure blob storage
            // Don't pick the file name from IFormFile in Production as it can be malicious and can be a potential vulnerability.
            string url = await _blobService.UploadBlobAsync(readStream, fileName, containerName, cancellationToken);

            _logger.LogInformation($"Upload to Azure blob storage successful with location - {url}");

            // Filename acts as claim check
            ClaimCheck message = new()
            {
                FileName = fileName,
                BlobLocation = url,
                ProcessingTime = DateTime.UtcNow
            };

            await _messagePublisher.PublishMessageAsync(
                message,
                messageId: requestId,
                correlationId: correlationId,
                cancellationToken: cancellationToken
            );

            return Accepted(message);
        }
    }
}
