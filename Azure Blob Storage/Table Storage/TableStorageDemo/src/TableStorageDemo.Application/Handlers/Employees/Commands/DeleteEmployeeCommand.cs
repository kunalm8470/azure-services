using MediatR;

namespace TableStorageDemo.Application.Handlers.Employees.Commands
{
    public class DeleteEmployeeCommand : IRequest<Unit>
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string ETag { get; set; }
    }
}
