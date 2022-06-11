using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TodoAPI.Application.Handlers.Todos.Commands;
using TodoAPI.Application.Handlers.Todos.Queries;
using TodoAPI.Domain.Entities;
using TodoAPI.Domain.Exceptions;
using TodoAPIMongo.Models.Requests;
using TodoAPIMongo.Models.Responses;

namespace TodoAPIMongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<PagedTodo>> List([FromQuery] ListTodo request)
        {
            var (todos, count) = await _mediator.Send(new ListTodosQuery
            {
                Page = request.Page,
                Limit = request.Limit,
            }).ConfigureAwait(false);

            PagedTodo todosDto = new()
            {
                Total = count,
                Page = request.Page,
                Limit = request.Limit,
                Data = todos.Select(x => new TodoModel
                {
                    Id = x.Id.ToString(),
                    Title = x.Title,
                    Description = x.Description,
                    Completed = x.Completed
                })
            };

            return Ok(todosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetById([FromRoute] string id)
        {
            Todo? found = await _mediator.Send(new GetTodoByIdQuery { Id = id }).ConfigureAwait(false);

            if (found == default)
            {
                throw new ResourceNotFoundException();
            }

            TodoModel response = new TodoModel
            {
                Id = found.Id.ToString(),
                Title = found.Title,
                Description = found.Description,
                Completed = found.Completed
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodo createEmployee)
        {
            await _mediator.Send(new CreateTodoCommand
            {
                Todo = new Todo
                {
                    Title = createEmployee.Title,
                    Description = createEmployee.Description
                }
            }).ConfigureAwait(false);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]
        public async Task<ActionResult<TodoModel>> Update([FromBody] UpdateTodo updateTodo)
        {
            Todo updated = await _mediator.Send(new UpdateTodoCommand
            {
                Todo = new Todo
                {
                    Id = new ObjectId(updateTodo.Id),
                    Title = updateTodo.Title,
                    Description = updateTodo.Description,
                    Completed = updateTodo.Completed
                }
            }).ConfigureAwait(false);

            TodoModel mapped = new TodoModel
            {
                Id = updated.Id.ToString(),
                Title = updated.Title,
                Description = updated.Description,
                Completed = updated.Completed
            };

            return Ok(mapped);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            await _mediator.Send(new DeleteTodoCommand { Id = id }).ConfigureAwait(false);

            return NoContent();
        }
    }
}
