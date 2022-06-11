using MediatR;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Application.Handlers.Queries
{
    public class GetTodoByIdQuery : IRequest<Todo?>
    {
        public int Id { get; set; }
    }
}
