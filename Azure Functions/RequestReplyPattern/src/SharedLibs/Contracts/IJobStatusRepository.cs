using Azure;
using SharedLibs.Models;

namespace SharedLibs.Contracts
{
    public interface IJobStatusRepository
    {
        public Task<Job> GetAsync(string partitionKey, string rowKey, CancellationToken token = default);

        public Task<Response> AddAsync(Job item, CancellationToken token = default);

        public Task<Response> UpdateAsync(Job item, CancellationToken token = default);
    }
}
