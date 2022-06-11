using MediatR;
using TodoAPI.Domain.Contracts;

namespace TodoAPI.Application.Handlers.Todos.Commands
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
            await _todoRepository.UpdateAsync(command.Todo).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}
