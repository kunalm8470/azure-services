using MediatR;
using TodoAPI.Domain.Contracts;

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
            await _todoRepository.DeleteAsync(request.Id).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
