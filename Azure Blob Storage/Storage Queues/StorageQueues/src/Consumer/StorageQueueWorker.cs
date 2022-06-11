using Azure;
using Azure.Storage.Queues.Models;
using SharedLibs.Contracts;
using SharedLibs.Models;
using System.Text.Json;

namespace Consumer
{
    public class StorageQueueWorker : BackgroundService
    {
        private readonly ILogger<StorageQueueWorker> _logger;
        private readonly IStorageQueueService _queueService;

        public StorageQueueWorker(ILogger<StorageQueueWorker> logger, IStorageQueueService queueService)
        {
            _logger = logger;
            _queueService = queueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Starting reading from queue running at: {time}", DateTimeOffset.Now);
                
                await Task.Delay(1000, stoppingToken);

                await ProcessStorageQueueMessageAsync(stoppingToken).ConfigureAwait(false);
            }
        }

        private async Task ProcessStorageQueueMessageAsync(CancellationToken stoppingToken)
        {
            Response<QueueMessage> queueMessage = await _queueService.ReceiveMessageAsync(cancellationToken: stoppingToken).ConfigureAwait(false);

            if (queueMessage.Value == default)
            {
                return;
            }

            string messageId = queueMessage.Value.MessageId;
            string popReceipt = queueMessage.Value.PopReceipt;

            try
            {
                Todo model = queueMessage.Value.Body.ToObjectFromJson<Todo>();

                _logger.LogInformation("New message read\n Id: {0}, Title: {1}, Description: {1}, Completed: {2}", model.Id, model.Title, model.Description, model.Completed);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing message", queueMessage.Value);

                // Publishing message to poison queue
                await _queueService.DeadLetterMessageAsync(queueMessage.Value.Body).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message", queueMessage.Value);

                // Publishing message to poison queue
                await _queueService.DeadLetterMessageAsync(queueMessage.Value.Body).ConfigureAwait(false);
            }
            finally
            {
                await _queueService.DeleteMessageAsync(messageId, popReceipt, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}