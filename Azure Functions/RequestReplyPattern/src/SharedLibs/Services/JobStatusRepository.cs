using Azure;
using Azure.Data.Tables;
using SharedLibs.Contracts;
using SharedLibs.Models;

namespace SharedLibs.Services
{
    public class JobStatusRepository : IJobStatusRepository
    {
        private readonly TableClient _client;

        public JobStatusRepository(TableClient client)
        {
            _client = client;
        }

        public async Task<Response> AddAsync(Job item, CancellationToken token = default)
        {
            return await _client.AddEntityAsync<Job>(item, token);
        }

        public async Task<Response> UpdateAsync(Job item, CancellationToken token = default)
        {
            return await _client.UpdateEntityAsync<Job>(item, item.ETag, TableUpdateMode.Replace, cancellationToken: token);
        }

        public async Task<Job> GetAsync(string partitionKey, string rowKey, CancellationToken token)
        {
            return await _client.GetEntityAsync<Job>(partitionKey, rowKey, cancellationToken: token);
        }
    }
}
