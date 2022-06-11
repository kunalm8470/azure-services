using System.Net;
using HttpTriggeredIsolatedProcessDemo.Models;
using HttpTriggeredIsolatedProcessDemo.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace HttpTriggeredIsolatedProcessDemo
{
    public class HttpTriggeredIsolatedFunction
    {
        private readonly ILogger _logger;
        private readonly JsonTypicodeService _service;

        public HttpTriggeredIsolatedFunction(ILoggerFactory loggerFactory,
            JsonTypicodeService service)
        {
            _logger = loggerFactory.CreateLogger<HttpTriggeredIsolatedFunction>();
            _service = service;
        }

        [Function("HttpTriggeredIsolatedFunction")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userId:int}")] HttpRequestData req, 
            int userId)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            User? found = await _service.GetUserAsync(userId);

            if (found == default)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(found);

            return response;
        }
    }
}
