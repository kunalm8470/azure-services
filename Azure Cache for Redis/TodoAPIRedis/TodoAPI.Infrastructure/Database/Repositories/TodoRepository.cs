using Newtonsoft.Json;
using StackExchange.Redis;
using TodoAPI.Domain.Contracts;
using TodoAPI.Domain.Entities;
using TodoAPI.Domain.Exceptions;

namespace TodoAPI.Infrastructure.Database.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IDatabase _db;

        public TodoRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = connectionMultiplexer.GetDatabase();
        }

        public async Task AddAsync(Todo item)
        {
            RedisKey key = "todoitems";

            RedisValue hash = $"todo:{item.Id}";

            RedisValue value = JsonConvert.SerializeObject(item);

            /*
                No support for cancellation tokens yet
                See issue https://github.com/StackExchange/StackExchange.Redis/issues/1039
            */
            await _db.HashSetAsync(key, hash, value).ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            RedisKey key = "todoitems";

            RedisValue hash = $"todo:{id}";

            if (!await _db.HashExistsAsync(key, hash).ConfigureAwait(false))
            {
                throw new ResourceNotFoundException($"Todo item not found with id {id}");
            }

            // See https://redis.io/commands/HDEL
            await _db.HashDeleteAsync(key, hash).ConfigureAwait(false);
        }

        public async Task<Todo> GetByIdAsync(int id)
        {
            RedisKey key = "todoitems";

            RedisValue hash = $"todo:{id}";

            if (!await _db.HashExistsAsync(key, hash).ConfigureAwait(false))
            {
                throw new ResourceNotFoundException($"Todo item not found with id {id}");
            }

            string value = await _db.HashGetAsync(key, hash).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Todo>(value);
        }

        public async Task<(IEnumerable<Todo>, long)> ListAsync(int skip, int limit)
        {
            RedisKey key = "todoitems";

            // Fetch total number of hashes using key
            // See https://redis.io/commands/hlen
            long count = await _db.HashLengthAsync(key).ConfigureAwait(false);
            
            if (count == 0)
            {
                return (Enumerable.Empty<Todo>(), count);
            }

            // Form the page of hashes needed
            RedisValue[] hashes = (await _db.HashKeysAsync(key).ConfigureAwait(false)).Skip(skip).Take(limit).ToArray();

            // Get the required values at once
            // See https://redis.io/commands/HMGET
            RedisValue[] values = await _db.HashGetAsync(key, hashes).ConfigureAwait(false);

            List<Todo> todos = values
                .Select(x => JsonConvert.DeserializeObject<Todo>(x))
                .ToList();

            return (todos, count);
        }

        public async Task UpdateAsync(Todo item)
        {
            RedisKey key = "todoitems";

            RedisValue hash = $"todo:{item.Id}";

            if (!await _db.HashExistsAsync(key, hash).ConfigureAwait(false))
            {
                throw new ResourceNotFoundException($"Todo item not found with id {item.Id}");
            }

            RedisValue value = JsonConvert.SerializeObject(item);

            await _db.HashSetAsync(key, hash, value).ConfigureAwait(false);
        }
    }
}
