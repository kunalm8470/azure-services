using FluentValidation;
using TableStorageDemo.API.Models.Requests;

namespace TableStorageDemo.API.Validators.Employees
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployee>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(e => e.Id)
            .NotNull()
            .WithMessage("Employee Id can't be empty");

            RuleFor(e => e.FirstName)
            .NotNull()
            .MinimumLength(2)
            .WithMessage("First name must be at least 2 character long");

            RuleFor(e => e.FirstName)
            .MaximumLength(200)
            .WithMessage("First name must be less than 200 characters");

            RuleFor(e => e.LastName)
            .NotNull()
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 character long");

            RuleFor(e => e.LastName)
            .MaximumLength(200)
            .WithMessage("Last name must be less than 200 characters");

            RuleFor(e => e.Email)
            .NotNull()
            .WithMessage("Email can't be empty");

            RuleFor(e => e.Email)
            .EmailAddress()
            .WithMessage("Invalid Email");

            RuleFor(e => e.Department)
            .NotNull()
            .MinimumLength(2)
            .WithMessage("Department must be at least 2 character long");

            RuleFor(e => e.Department)
            .MaximumLength(200)
            .WithMessage("Department must be less than 200 characters");

            RuleFor(e => e.PartitionKey)
            .NotNull()
            .WithMessage("Partition key can't be empty");

            RuleFor(e => e.RowKey)
            .NotNull()
            .WithMessage("Row key can't be empty");

            RuleFor(e => e.Etag)
            .NotNull()
            .WithMessage("ETag can't be empty");
        }
    }
}
