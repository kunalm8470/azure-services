using MediatR;
using TodoAPI.Domain.Contracts;
using TodoAPI.Domain.Entities;
using TodoAPI.Domain.Exceptions;

namespace TodoAPI.Application.Handlers.Commands
{
    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, Unit>
    {
        private readonly ITodoRepository _todoRepository;
        public UpdateTodoCommandHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Unit> Handle(UpdateTodoCommand command, CancellationToken token)
        {
            Todo? found = await _todoRepository.GetByIdAsync(command.Todo.Id).ConfigureAwait(false);

            if (found == default)
            {
                throw new ResourceNotFoundException($"Todo item with {command.Todo.Id} not found!");
            }

            await _todoRepository.UpdateAsync(command.Todo, token);
            return Unit.Value;
        }
    }
}
