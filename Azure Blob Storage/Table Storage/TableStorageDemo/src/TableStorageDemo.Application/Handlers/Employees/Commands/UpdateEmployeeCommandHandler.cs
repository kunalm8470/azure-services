using Azure;
using MediatR;
using TableStorageDemo.Domain.Contracts;

namespace TableStorageDemo.Application.Handlers.Employees.Commands
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Response>
    {
        private readonly IEmployeeRepository _employeeRepository;
        public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Response> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            return await _employeeRepository.UpdateAsync(request.Employee, cancellationToken);
        }
    }
}
