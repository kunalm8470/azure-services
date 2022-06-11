using MediatR;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Application.Handlers.Queries
{
    public class ListTodosQuery : IRequest<(IEnumerable<Todo>, int)>
    {
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
