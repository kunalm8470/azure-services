using MongoDB.Bson;
using MongoDB.Driver;
using TodoAPI.Domain.Contracts;
using TodoAPI.Domain.Entities;

namespace TodoAPI.Infrastructure.Database.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly IMongoCollection<Todo> _todos;

        public TodoRepository(MongoDbConnectionFactory connectionFactory, string collectionName)
        {
            _todos = connectionFactory.GetCollection<Todo>(collectionName);
        }

        public async Task AddAsync(Todo item, CancellationToken token = default)
        {
            InsertOneOptions insertOneOptions = new()
            {
                BypassDocumentValidation = true
            };

            await _todos.InsertOneAsync(item, insertOneOptions, token).ConfigureAwait(false);
        }

        public async Task DeleteAsync(string id, CancellationToken token = default)
        {
            ObjectId todoId = new(id);
            await _todos.DeleteOneAsync(x => x.Id == todoId, token).ConfigureAwait(false);
        }

        public async Task<Todo> GetByIdAsync(string id, CancellationToken token = default)
        {
            ObjectId todoId = new(id);
            return await _todos.Find(x => x.Id == todoId).FirstOrDefaultAsync(token).ConfigureAwait(false);
        }

        public async Task<(IEnumerable<Todo>, long)> ListAsync(int skip, int limit, CancellationToken token = default)
        {
            long count = await _todos.EstimatedDocumentCountAsync(cancellationToken: token).ConfigureAwait(false);

            List<Todo> todos = await _todos.Find(f => true)
                .Skip(skip)
                .Limit(limit)
                .ToListAsync(token)
                .ConfigureAwait(false);

            return (todos, count);
        }

        public async Task<Todo> UpdateAsync(Todo updateModel, CancellationToken token = default)
        {
            FilterDefinition<Todo> updateFilter = Builders<Todo>.Filter.Eq(t => t.Id, updateModel.Id);
            FindOneAndReplaceOptions<Todo> updateOptions = new() 
            {
                ReturnDocument = ReturnDocument.After 
            };

            return await _todos.FindOneAndReplaceAsync(updateFilter, updateModel, updateOptions, token).ConfigureAwait(false);
        }
    }
}
