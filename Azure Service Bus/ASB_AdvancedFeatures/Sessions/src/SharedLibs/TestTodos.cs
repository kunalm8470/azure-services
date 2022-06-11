using SharedLibs.Models;

namespace SharedLibs
{
    public static class TestTodos
    {
        public static List<Todo> GenerateTestData()
        {
            return new()
            {
                new()
                {
                    Id = 1,
                    Title = "delectus aut autem",
                    Completed = false
                },
                new()
                {
                    Id = 2,
                    Title = "quis ut nam facilis et officia qui",
                    Completed = false
                },
                new()
                {
                    Id = 3,
                    Title = "fugiat veniam minus",
                    Completed = false
                },
                new()
                {
                    Id = 4,
                    Title = "et porro tempora",
                    Completed = false
                },
                new()
                {
                    Id = 5,
                    Title = "laboriosam mollitia et enim quasi adipisci quia provident illum",
                    Completed = false
                },
                new()
                {
                    Id = 6,
                    Title = "qui ullam ratione quibusdam voluptatem quia omnis",
                    Completed = false
                },
                new()
                {
                    Id = 7,
                    Title = "illo expedita consequatur quia in",
                    Completed = false
                },
                new()
                {
                    Id = 8,
                    Title = "quo adipisci enim quam ut ab",
                    Completed = false
                },
                new()
                {
                    Id = 9,
                    Title = "molestiae perspiciatis ipsa",
                    Completed = false
                },
                new()
                {
                    Id = 10,
                    Title = "illo est ratione doloremque quia maiores aut",
                    Completed = false
                }
            };
        }
    }
}
