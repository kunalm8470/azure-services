using TodoAPI.Domain.Entities;

namespace TodoAPI.Domain.Contracts
{
    public interface ITodoRepository
    {
        public Task<(IEnumerable<Todo>, long)> ListAsync(int skip, int limit, CancellationToken token = default);
        public Task<Todo> GetByIdAsync(string id, CancellationToken token = default);
        public Task AddAsync(Todo item, CancellationToken token = default);
        public Task<Todo> UpdateAsync(Todo item, CancellationToken token = default);
        public Task DeleteAsync(string id, CancellationToken token = default);
    }
}
