using FluentValidation;
using TableStorageDemo.API.Models.Requests;

namespace TableStorageDemo.API.Validators.Employees
{
    public class DeleteEmployeeValidator : AbstractValidator<DeleteEmployeeQuery>
    {
        public DeleteEmployeeValidator()
        {
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
