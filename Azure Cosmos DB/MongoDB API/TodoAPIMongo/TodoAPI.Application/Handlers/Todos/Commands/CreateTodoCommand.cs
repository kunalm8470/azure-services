using MediatR;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Application.Handlers.Todos.Commands
{
    public class CreateTodoCommand : IRequest<Unit>
    {
        public Todo Todo { get; set; }
    }
}
