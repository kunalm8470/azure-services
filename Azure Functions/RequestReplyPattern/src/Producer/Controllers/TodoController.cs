using Microsoft.AspNetCore.Mvc;
using Producer.Models.Requests;
using SharedLibs.Contracts;
using SharedLibs.Models;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<TodoController> _logger;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IConfiguration _configuration;
        private readonly IJobStatusRepository _jobStatusRepository;

        public TodoController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IMessagePublisher messagePublisher, IJobStatusRepository jobStatusRepository, ILogger<TodoController> logger)
        {
            _messagePublisher = messagePublisher;
            _jobStatusRepository = jobStatusRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor), "HTTP context can't be null");
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodoItem([FromBody] TodoDto item, CancellationToken cancellationToken)
        {
            string requestId = Guid.NewGuid().ToString();
            string? correlationId = _httpContextAccessor.HttpContext?.Request.Headers["x-correlation-id"];

            _logger.LogInformation($"Request Id: {requestId}");
            _logger.LogInformation($"Correlation Id: {correlationId}");

            Todo todo = new()
            {
                Id = requestId,
                Title = item.Title,
                Description = item.Description,
                Completed = false
            };

            string statusUrl = $"{_configuration["StatusFunctionHostname"]}/api/status/{correlationId}?jobId={requestId}";

            Job newjob = new()
            {
                Id = requestId,
                CorrelationId = correlationId,
                StatusUrl = statusUrl,
                Status = JobConstants.PENDING,
                PartitionKey = correlationId,
                RowKey = requestId
            };

            await _jobStatusRepository.AddAsync(newjob, cancellationToken);

            await _messagePublisher.PublishMessageAsync(
                todo,
                messageId: requestId,
                correlationId: correlationId,
                cancellationToken: cancellationToken
            );

            // Set location header as status url for discoverability
            return Accepted(statusUrl, todo);
        }
    }
}
