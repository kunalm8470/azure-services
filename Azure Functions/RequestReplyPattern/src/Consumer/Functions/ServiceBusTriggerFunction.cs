using System;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SharedLibs.Contracts;
using SharedLibs.Models;

namespace Consumer
{
    public class ServiceBusTriggerFunction
    {
        private readonly ILogger<ServiceBusTriggerFunction> _logger;
        private readonly IJobStatusRepository _jobStatusRepository;

        public ServiceBusTriggerFunction(IJobStatusRepository jobStatusRepository, ILogger<ServiceBusTriggerFunction> logger)
        {
            _logger = logger;
            _jobStatusRepository = jobStatusRepository;
        }

        [FunctionName("ServiceBusTriggerFunction")]
        public async Task Run([ServiceBusTrigger("todoqueue", Connection = "todoqueueConnection")] ServiceBusReceivedMessage receivedMessage)
        {
            Job processingJob;
            string partitionKey = receivedMessage.CorrelationId;
            string rowKey = receivedMessage.MessageId;

            // Mark job as processing
            processingJob = await _jobStatusRepository.GetAsync(partitionKey, rowKey);
            processingJob.Status = JobConstants.PROCESSING;
            await _jobStatusRepository.UpdateAsync(processingJob);

            //throw new Exception("Simply");

            Todo item = receivedMessage.Body.ToObjectFromJson<Todo>();

            await Task.Delay(TimeSpan.FromMinutes(1));

            _logger.LogInformation($"Message Id: {receivedMessage.MessageId}");
            _logger.LogInformation($"Correlation Id: {receivedMessage.CorrelationId}");
            _logger.LogInformation("Todo Id: {0}, Todo title: {1}, Todo description: {2}, Todo completed: {3}", item.Id, item.Title, item.Description, item.Completed);

            // Mark job as processing
            processingJob = await _jobStatusRepository.GetAsync(partitionKey, rowKey);
            processingJob.Status = JobConstants.COMPLETED;
            await _jobStatusRepository.UpdateAsync(processingJob);
        }

        [FunctionName("ServiceBusTriggerFunctionDlq")]
        public async Task RunDeadLetterQueue([ServiceBusTrigger("todoqueue/$deadletterqueue", Connection = "todoqueueConnectionDlq")] ServiceBusReceivedMessage receivedMessage)
        {
            _logger.LogWarning($"Dead lettered message delivery count: {receivedMessage.DeliveryCount}");
            _logger.LogWarning($"Dead lettered metadata: {JsonSerializer.Serialize(receivedMessage.ApplicationProperties, new JsonSerializerOptions { WriteIndented = true })}");

            string partitionKey = receivedMessage.CorrelationId;
            string rowKey = receivedMessage.MessageId;

            Job currentJob = await _jobStatusRepository.GetAsync(partitionKey, rowKey);
            if (currentJob != default)
            {
                currentJob.Status = JobConstants.FAILED;
                await _jobStatusRepository.UpdateAsync(currentJob);
            }
        }
    }
}
