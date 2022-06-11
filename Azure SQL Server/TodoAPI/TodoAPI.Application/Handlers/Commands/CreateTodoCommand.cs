using MediatR;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Application.Handlers.Commands
{
    public class CreateTodoCommand : IRequest<Unit>
    {
        public Todo Todo { get; set; }
    }
}
