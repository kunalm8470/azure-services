using MediatR;
using TodoAPI.Domain.Contracts;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Application.Handlers.Todos.Queries
{
    public class ListTodosQueryHandler : IRequestHandler<ListTodosQuery, (IEnumerable<Todo>, long)>
    {
        private readonly ITodoRepository _todoRepository;
        public ListTodosQueryHandler(ITodoRepository employeeRepository)
        {
            _todoRepository = employeeRepository;
        }

        public async Task<(IEnumerable<Todo>, long)> Handle(ListTodosQuery request, CancellationToken cancellationToken)
        {
            int skip = (request.Page - 1) * request.Limit;

            return await _todoRepository.ListAsync(skip, request.Limit, cancellationToken).ConfigureAwait(false);
        }
    }
}
