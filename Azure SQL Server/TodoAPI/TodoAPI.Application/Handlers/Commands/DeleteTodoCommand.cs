using MediatR;

namespace TodoAPI.Application.Handlers.Commands
{
    public class DeleteTodoCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
