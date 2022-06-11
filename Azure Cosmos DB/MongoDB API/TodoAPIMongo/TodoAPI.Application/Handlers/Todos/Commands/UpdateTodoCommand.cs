using MediatR;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Application.Handlers.Todos.Commands
{
    public class UpdateTodoCommand : IRequest<Todo>
    {
        public Todo Todo { get; set; }
    }
}
