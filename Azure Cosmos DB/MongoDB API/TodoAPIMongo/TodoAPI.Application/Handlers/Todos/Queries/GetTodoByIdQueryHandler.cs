using MediatR;
using TodoAPI.Domain.Contracts;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Application.Handlers.Todos.Queries
{
    public class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, Todo?>
    {
        private readonly ITodoRepository _todoRepository;
        public GetTodoByIdQueryHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Todo?> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
        {
            return await _todoRepository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        }
    }
}
