using TodoAPI.Domain.Entities;

namespace TodoAPI.Domain.Contracts
{
    public interface ITodoRepository
    {
        public Task<(IEnumerable<Todo>, long)> ListAsync(int skip, int limit);
        public Task<Todo> GetByIdAsync(int id);
        public Task AddAsync(Todo item);
        public Task UpdateAsync(Todo item);
        public Task DeleteAsync(int id);
    }
}
