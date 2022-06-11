using MediatR;
using TableStorageDemo.Domain.Contracts;
using TableStorageDemo.Domain.Entities;

namespace TableStorageDemo.Application.Handlers.Employees.Queries
{
    public class GetEmployeeByPartitionKeyAndRowKeyQueryHandler : IRequestHandler<GetEmployeeByPartitionKeyAndRowKeyQuery, Employee?>
    {
        private readonly IEmployeeRepository _employeeRepository;
        public GetEmployeeByPartitionKeyAndRowKeyQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Employee?> Handle(GetEmployeeByPartitionKeyAndRowKeyQuery request, CancellationToken cancellationToken)
        {
            return await _employeeRepository.GetAsync(request.PartitionKey, request.RowKey, cancellationToken);
        }
    }
}
