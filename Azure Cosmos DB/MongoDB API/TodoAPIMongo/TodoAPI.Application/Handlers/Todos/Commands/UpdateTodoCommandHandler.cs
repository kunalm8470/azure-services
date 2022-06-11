using MediatR;
using TodoAPI.Domain.Contracts;
using TodoAPI.Domain.Entities;
using TodoAPI.Domain.Exceptions;

namespace TodoAPI.Application.Handlers.Todos.Commands
{
    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, Todo>
    {
        private readonly ITodoRepository _todoRepository;
        public UpdateTodoCommandHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Todo> Handle(UpdateTodoCommand command, CancellationToken token)
        {
            Todo? found = await _todoRepository.GetByIdAsync(command.Todo.Id.ToString(), token).ConfigureAwait(false);

            if (found == default)
            {
                throw new ResourceNotFoundException($"Todo item with {command.Todo.Id} not found!");
            }

            return await _todoRepository.UpdateAsync(command.Todo, token);
        }
    }
}
