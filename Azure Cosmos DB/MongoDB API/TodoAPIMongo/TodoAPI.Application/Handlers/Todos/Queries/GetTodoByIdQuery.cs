using MediatR;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Application.Handlers.Todos.Queries
{
    public class GetTodoByIdQuery : IRequest<Todo?>
    {
        public string Id { get; set; }
    }
}
