using Azure;
using MediatR;
using TableStorageDemo.Domain.Contracts;
using TableStorageDemo.Domain.Entities;

namespace TableStorageDemo.Application.Handlers.Employees.Commands
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Response>
    {
        private readonly IEmployeeRepository _employeeRepository;
        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Response> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            return await _employeeRepository.AddAsync(request.Employee, cancellationToken);
        }
    }
}
