using Azure;
using TableStorageDemo.Domain.Entities;

namespace TableStorageDemo.Domain.Contracts
{
    public interface IEmployeeRepository
    {
        public Task<Employee> GetAsync(string partitionKey, string rowKey, CancellationToken token = default);

        public Task<Response> AddAsync(Employee item, CancellationToken token = default);

        public Task<Response> UpdateAsync(Employee item, CancellationToken token = default);

        public Task DeleteAsync(string partitionKey, string rowKey, string? ifMatch = default, CancellationToken token = default);
    }
}
