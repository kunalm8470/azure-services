using MediatR;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Application.Handlers.Commands
{
    public class UpdateTodoCommand : IRequest<Unit>
    {
        public Todo Todo { get; set; }
    }
}
