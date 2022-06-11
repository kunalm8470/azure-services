using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using HttpTriggerFunctionDemo.Services;
using HttpTriggerFunctionDemo.Models;

namespace HttpTriggerFunctionDemo
{
    public class HttpTriggerFunction
    {
        private readonly ILogger<HttpTriggerFunction> _logger;
        private readonly JsonTypicodeService _service;

        public HttpTriggerFunction(ILogger<HttpTriggerFunction> logger, JsonTypicodeService service)
        {
            _logger = logger;
            _service = service;
        }

        [FunctionName("HttpTriggerFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{id}")] HttpRequest req,
            int id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            User found = await _service.GetUserAsync(id);

            if (found == default)
            {
                return new NotFoundObjectResult(new { message = $"User not found with id {id}" });
            }

            return new OkObjectResult(found);
        }
    }
}
