using FluentValidation;
using TodoAPIMongo.Models.Requests;

namespace TodoAPIMongo.Validators.Todo
{
    public class ListTodoValidator : AbstractValidator<ListTodo>
    {
        public ListTodoValidator()
        {
            RuleFor(r => r.Page)
            .NotNull()
            .Must(x => x > 0)
            .WithMessage("Page cannot be less than 0");

            RuleFor(r => r.Limit)
            .NotNull()
            .Must(x => x > 0)
            .WithMessage("Limit cannot be less than 0");
        }
    }
}
