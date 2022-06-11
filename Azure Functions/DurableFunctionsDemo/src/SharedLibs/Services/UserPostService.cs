using SharedLibs.Contracts;
using SharedLibs.Models;
using System.Text.Json;

namespace SharedLibs.Services
{
    public class UserPostService : IUserPostService
    {
        private readonly HttpClient _httpClient;

        public UserPostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Post>> FetchPostsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"posts/?userId={userId}", cancellationToken);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return Enumerable.Empty<Post>();
            }

            string serialized = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<IEnumerable<Post>>(serialized);
        }
    }
}
