using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using FunctionApp.Models.Requests;

namespace FunctionApp.Functions
{
    public class StarterFunction
    {
        private readonly ILogger<StarterFunction> _logger;

        public StarterFunction(ILogger<StarterFunction> logger)
        {
            _logger = logger;
        }

        [FunctionName("StarterFunction")]
        public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{userId}")] HttpRequestMessage req,
            int userId,
            [DurableClient] IDurableOrchestrationClient starter
        )
        {
            _logger.LogInformation("Running the starter function now!");

            OrchestratorFunctionRequest request = new()
            {
                UserId = userId
            };

            string instanceId = await starter.StartNewAsync<OrchestratorFunctionRequest>("OrchestratorFunction", request);

            _logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
