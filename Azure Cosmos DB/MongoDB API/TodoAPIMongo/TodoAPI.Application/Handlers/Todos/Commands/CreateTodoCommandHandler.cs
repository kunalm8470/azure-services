using MediatR;
using TodoAPI.Domain.Contracts;

namespace TodoAPI.Application.Handlers.Todos.Commands
{
    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Unit>
    {
        private readonly ITodoRepository _todoRepository;
        public CreateTodoCommandHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Unit> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            await _todoRepository.AddAsync(request.Todo, cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
