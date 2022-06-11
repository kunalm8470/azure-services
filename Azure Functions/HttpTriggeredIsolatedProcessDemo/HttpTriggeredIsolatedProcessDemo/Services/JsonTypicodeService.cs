using HttpTriggeredIsolatedProcessDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpTriggeredIsolatedProcessDemo.Services
{
    public class JsonTypicodeService
    {
        private readonly HttpClient _httpClient;
        public JsonTypicodeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User?> GetUserAsync(int id, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"users/{id}", cancellationToken);

            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }

            string serialized = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User?>(serialized);
        }
    }
}
