using MongoDB.Bson;

namespace TodoAPI.Domain.Entities
{
    public class Todo
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}
