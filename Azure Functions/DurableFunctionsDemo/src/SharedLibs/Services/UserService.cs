using SharedLibs.Contracts;
using SharedLibs.Models;
using System.Text.Json;

namespace SharedLibs.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User?> FetchUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"users/{id}", cancellationToken);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }

            string serialized = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
            return JsonSerializer.Deserialize<User>(serialized);
        }
    }
}
