using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharedLibs.Contracts;
using SharedLibs.Models;

namespace Consumer
{
    public class StatusCheckerFunction
    {
        private readonly ILogger<StatusCheckerFunction> _logger;
        private readonly IJobStatusRepository _jobStatusRepository;

        public StatusCheckerFunction(IJobStatusRepository jobStatusRepository, ILogger<StatusCheckerFunction> logger)
        {
            _logger = logger;
            _jobStatusRepository = jobStatusRepository;
        }

        [FunctionName("StatusCheckerFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "status/{requestId}")] HttpRequest req,
            string requestId)
        {
            _logger.LogInformation("C# HTTP trigger function is starting processing a request.");

            string jobId = req.Query["jobId"];

            Job current = await _jobStatusRepository.GetAsync(requestId, jobId);

            if (current == default)
            {
                return new NotFoundObjectResult(new { message = "Job not existing yet" });
            }

            return new OkObjectResult(current);
        }
    }
}
