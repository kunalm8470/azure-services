using MongoDB.Driver;

namespace TodoAPI.Infrastructure
{
    public class MongoDbConnectionFactory
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoDbConnectionFactory(string connectionString, string databaseName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
