using Azure;
using Azure.Data.Tables;
using TableStorageDemo.Domain.Contracts;
using TableStorageDemo.Domain.Entities;

namespace TableStorageDemo.Infrastructure.TableStorage.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly TableClient _client;

        public EmployeeRepository(TableClient client)
        {
            _client = client;
        }

        public async Task<Response> AddAsync(Employee item, CancellationToken token = default)
        {
            return await _client.AddEntityAsync<Employee>(item, token);
        }

        public async Task DeleteAsync(string partitionKey, string rowKey, string? ifMatch = null, CancellationToken token = default)
        {
            ETag eTag = default;
            if (!string.IsNullOrEmpty(ifMatch))
            {
                eTag = new(ifMatch);
            }

            await _client.DeleteEntityAsync(partitionKey, rowKey, eTag, token);
        }

        public async Task<Employee> GetAsync(string partitionKey, string rowKey, CancellationToken token = default)
        {
            return await _client.GetEntityAsync<Employee>(partitionKey, rowKey, cancellationToken: token);
        }

        public async Task<Response> UpdateAsync(Employee item, CancellationToken token = default)
        {
            return await _client.UpdateEntityAsync<Employee>(item, item.ETag, TableUpdateMode.Replace, cancellationToken: token);
        }
    }
}
