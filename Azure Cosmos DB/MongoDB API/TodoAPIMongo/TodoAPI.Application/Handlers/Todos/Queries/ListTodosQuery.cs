using MediatR;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Application.Handlers.Todos.Queries
{
    public class ListTodosQuery : IRequest<(IEnumerable<Todo>, long)>
    {
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
