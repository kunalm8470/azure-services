using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Producer.Contracts;
using Producer.Models.Requests;
using SharedLibs.Models;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IMessagePublisher _messagePublisher;
        public TodoController(IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodoItem([FromBody] TodoDto item, CancellationToken cancellationToken)
        {
            Todo todo = new()
            {
                Id = Guid.NewGuid(),
                Title = item.Title,
                Description = item.Description,
                Completed = false
            };

            await _messagePublisher.PublishMessageAsync(todo, cancellationToken: cancellationToken);
            return Accepted(todo);
        }
    }
}
