using SharedLibs.Contracts;
using SharedLibs.Models;
using System.Text.Json;

namespace SharedLibs.Services
{
    public class UserPostCommentService : IUserPostCommentService
    {
        private readonly HttpClient _httpClient;

        public UserPostCommentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Comment>> FetchCommentsByUserPostId(IEnumerable<int> postIds, CancellationToken cancellationToken = default)
        {
            IEnumerable<Task<HttpResponseMessage>> fetchCommentTasks = postIds.Select(postId => _httpClient.GetAsync($"comments/?postId={postId}", cancellationToken));

            await Task.WhenAll(fetchCommentTasks);

            List<Comment> comments = new();

            foreach (Task<HttpResponseMessage> fetchCommentTask in fetchCommentTasks)
            {
                HttpResponseMessage commentResponse = await fetchCommentTask;
                string serialized = await commentResponse.Content.ReadAsStringAsync(cancellationToken);

                comments.AddRange(JsonSerializer.Deserialize<IEnumerable<Comment>>(serialized));
            }

            return comments;
        }
    }
}
