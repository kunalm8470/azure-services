using MediatR;

namespace TodoAPI.Application.Handlers.Todos.Commands
{
    public class DeleteTodoCommand : IRequest<Unit>
    {
        public string Id { get; set; }
    }
}
