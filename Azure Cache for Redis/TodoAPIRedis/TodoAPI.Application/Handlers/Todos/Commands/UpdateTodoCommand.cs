using MediatR;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Application.Handlers.Todos.Commands
{
    public class UpdateTodoCommand : IRequest<Unit>
    {
        public Todo Todo { get; set; }
    }
}
