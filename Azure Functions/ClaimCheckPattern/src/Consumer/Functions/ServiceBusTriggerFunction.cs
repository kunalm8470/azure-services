using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedLibs.Contracts;
using SharedLibs.Extensions;
using SharedLibs.Models;

namespace Consumer.Functions
{
    public class ServiceBusTriggerFunction
    {
        private readonly IBlobService _blobService;
        private readonly ILogger<ServiceBusTriggerFunction> _logger;
        private readonly IConfiguration _configuration;

        public ServiceBusTriggerFunction(
            IBlobService blobService, 
            ILogger<ServiceBusTriggerFunction> logger,
            IConfiguration configuration
        )
        {
            _blobService = blobService;
            _logger = logger;
            _configuration = configuration;
        }

        [FunctionName("ServiceBusTriggerFunction")]
        public async Task Run([ServiceBusTrigger("employees-claim-check-queue", Connection = "serviceBusConnection")] ServiceBusReceivedMessage receivedMessage)
        {
            ClaimCheck message = receivedMessage.Body.ToObjectFromJson<ClaimCheck>();

            string containerName = _configuration.GetValue<string>("BlobContainer");

            Stream uploaded = await _blobService.GetBlobAsync(containerName, message.FileName);

            _logger.LogInformation("Starting processing employees");

            List<Employee> employees = new(); 
            await foreach (Employee emp in uploaded.StreamPerJsonObject<Employee>())
            {
                employees.Add(emp);
            }

            _logger.LogInformation("Finished processing employees");

            // Clean up data after processing
            await _blobService.DeleteBlobAsync(containerName, message.FileName);
        }

        [FunctionName("ServiceBusTriggerFunctionDlq")]
        public Task RunDeadLetterQueue([ServiceBusTrigger("employees-claim-check-queue/$deadletterqueue", Connection = "serviceBusConnectionDlq")] ServiceBusReceivedMessage receivedMessage)
        {
            _logger.LogWarning($"Dead lettered message delivery count: {receivedMessage.DeliveryCount}");
            _logger.LogWarning($"Dead lettered metadata: {JsonConvert.SerializeObject(receivedMessage.ApplicationProperties, Formatting.Indented)}");

            return Task.CompletedTask;
        }
    }
}
