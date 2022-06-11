using MediatR;
using TodoAPI.Domain.Contracts;
using TodoAPI.Domain.Entities;
using TodoAPI.Domain.Exceptions;

namespace TodoAPI.Application.Handlers.Todos.Commands
{
    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, Unit>
    {
        private readonly ITodoRepository _todoRepository;
        public DeleteTodoCommandHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Unit> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            Todo? found = await _todoRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);

            if (found == default)
            {
                throw new ResourceNotFoundException($"Todo item with {request.Id} not found!");
            }

            await _todoRepository.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
