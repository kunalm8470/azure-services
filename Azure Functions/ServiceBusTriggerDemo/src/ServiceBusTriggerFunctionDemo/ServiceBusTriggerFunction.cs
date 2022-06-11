using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SharedLibs.Models;

namespace ServiceBusTriggerFunctionDemo
{
    public class ServiceBusTriggerFunction
    {
        private readonly ILogger<ServiceBusTriggerFunction> _logger;

        public ServiceBusTriggerFunction(ILogger<ServiceBusTriggerFunction> logger)
        {
            _logger = logger;
        }

        [FunctionName("ServiceBusTriggerFunction")]
        public Task Run([ServiceBusTrigger("todoqueue", Connection = "todoqueueConnection")] ServiceBusReceivedMessage receivedMessage)
        {
            //Todo item = receivedMessage.Body.ToObjectFromJson<Todo>();

            //_logger.LogWarning($"Delivery count: {receivedMessage.DeliveryCount}");
            //_logger.LogInformation("MessageId: {0}", receivedMessage.MessageId);
            //_logger.LogInformation("Todo Id: {0}, Todo title: {1}, Todo description: {2}, Todo completed: {3}", item.Id, item.Title, item.Description, item.Completed);

            //return Task.CompletedTask;

            throw new System.Exception("Simply");
        }

        [FunctionName("ServiceBusTriggerFunctionDlq")]
        public Task RunDeadLetterQueue([ServiceBusTrigger("todoqueue/$deadletterqueue", Connection = "todoqueueConnectionDlq")] ServiceBusReceivedMessage receivedMessage)
        {
            _logger.LogWarning($"Dead lettered message delivery count: {receivedMessage.DeliveryCount}");
            _logger.LogWarning($"Dead lettered metadata: {JsonSerializer.Serialize(receivedMessage.ApplicationProperties, new JsonSerializerOptions { WriteIndented = true })}");

            return Task.CompletedTask;
        }
    }
}
