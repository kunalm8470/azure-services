using FluentValidation;
using TodoAPIMongo.Models.Requests;

namespace TodoAPIMongo.Validators.Todo
{
    public class CreateTodoValidator : AbstractValidator<CreateTodo>
    {
        public CreateTodoValidator()
        {
            RuleFor(e => e.Title)
            .NotNull()
            .WithMessage("Title must be at least 2 character long");

            RuleFor(e => e.Title)
            .MinimumLength(2)
            .WithMessage("Title must be at least 2 character long");

            RuleFor(e => e.Title)
            .MaximumLength(200)
            .WithMessage("Title must be less than 200 characters");

            RuleFor(e => e.Description)
            .NotNull()
            .WithMessage("Description must be at least 2 character long");

            RuleFor(e => e.Description)
            .MinimumLength(2)
            .WithMessage("Description must be at least 2 character long");

            RuleFor(e => e.Description)
            .MaximumLength(1000)
            .WithMessage("Description must be less than 1000 characters");
        }
    }
}
