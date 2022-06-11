using MediatR;
using TableStorageDemo.Domain.Entities;

namespace TableStorageDemo.Application.Handlers.Employees.Queries
{
    public class GetEmployeeByPartitionKeyAndRowKeyQuery : IRequest<Employee?>
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }
}
