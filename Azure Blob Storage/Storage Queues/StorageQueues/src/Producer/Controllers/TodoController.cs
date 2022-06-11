using Azure;
using Azure.Storage.Queues.Models;
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
        private readonly IStorageQueueService _storageQueueService;

        public TodoController(IStorageQueueService storageQueueService)
        {
            _storageQueueService = storageQueueService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<SendReceipt>>> CreateTodoItem([FromBody] TodoDto item, CancellationToken cancellationToken)
        {
            Todo todo = new()
            {
                Id = Guid.NewGuid(),
                Title = item.Title,
                Description = item.Description,
                Completed = false
            };

            Response<SendReceipt> sendReciept = await _storageQueueService.PublishMessageAsync(todo, cancellationToken: cancellationToken);
            return Accepted(sendReciept);
        }
    }
}
