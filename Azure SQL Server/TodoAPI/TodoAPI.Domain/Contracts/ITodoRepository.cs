using TodoAPI.Domain.Entities;

namespace TodoAPI.Domain.Contracts
{
    public interface ITodoRepository
    {
        public Task<(IEnumerable<Todo>, int)> ListAsync(int skip, int limit, CancellationToken token = default);
        public Task<Todo> GetByIdAsync(int id, CancellationToken token = default);
        public Task AddAsync(Todo item, CancellationToken token = default);
        public Task UpdateAsync(Todo item, CancellationToken token = default);
        public Task DeleteAsync(int id, CancellationToken token = default);
    }
}
