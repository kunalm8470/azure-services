﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Application.Handlers.Commands;
using TodoAPI.Application.Handlers.Queries;
using TodoAPI.Domain.Entities;
using TodoAPI.Domain.Exceptions;
using TodoAPI.Models.Requests;
using TodoAPI.Models.Responses;

namespace TodoAPI.Controllers
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
                Data = todos
            };

            return Ok(todosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetById([FromRoute] int id)
        {
            Todo? found = await _mediator.Send(new GetTodoByIdQuery { Id = id }).ConfigureAwait(false);

            if (found == default)
            {
                throw new ResourceNotFoundException();
            }

            return Ok(found);
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
        public async Task<IActionResult> Update([FromBody] UpdateTodo updateTodo)
        {
            await _mediator.Send(new UpdateTodoCommand
            {
                Todo = new Todo
                {
                    Id = updateTodo.Id,
                    Title = updateTodo.Title,
                    Description = updateTodo.Description,
                    Completed = updateTodo.Completed
                }
            }).ConfigureAwait(false);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _mediator.Send(new DeleteTodoCommand { Id = id }).ConfigureAwait(false);

            return NoContent();
        }
    }
}