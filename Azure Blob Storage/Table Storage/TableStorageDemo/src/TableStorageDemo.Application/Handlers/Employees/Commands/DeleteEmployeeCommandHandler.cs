using MediatR;
using TableStorageDemo.Domain.Contracts;

namespace TableStorageDemo.Application.Handlers.Employees.Commands
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
    {
        private readonly IEmployeeRepository _employeeRepository;
        public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            await _employeeRepository.DeleteAsync(request.PartitionKey, request.RowKey, request.ETag, cancellationToken);
            return Unit.Value;
        }
    }
}
