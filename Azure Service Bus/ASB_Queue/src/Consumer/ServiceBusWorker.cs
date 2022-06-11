using Azure.Messaging.ServiceBus;
using SharedLibs.Models;

namespace Consumer
{
    public class ServiceBusWorker : BackgroundService
    {
        private readonly ILogger<ServiceBusWorker> _logger;
        private readonly ServiceBusProcessor _processor;

        public ServiceBusWorker(
            ILogger<ServiceBusWorker> logger,
            ServiceBusProcessor processor
        )
        {
            _logger = logger;
            _processor = processor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Register message handlers
            _processor.ProcessMessageAsync += ProcessMessageHandler;
            _processor.ProcessErrorAsync += ProcessExceptionHandler;

            // start processing
            await Task.Delay(1000, stoppingToken).ConfigureAwait(false);
            await _processor.StartProcessingAsync(stoppingToken).ConfigureAwait(false);
        }

        private Task ProcessExceptionHandler(ProcessErrorEventArgs args)
        {
            // Message will be auto-abandoned and tried to redelivered
            _logger.LogError(args.Exception, "Couldn't process message because: {0}", args.Exception.Message);
            return Task.CompletedTask;
        }

        private async Task ProcessMessageHandler(ProcessMessageEventArgs args)
        {
            string messageId = args.Message.MessageId;
            string? messageType = args.Message.ApplicationProperties["messageType"]?.ToString();
            Todo recieved = args.Message.Body.ToObjectFromJson<Todo>();

            _logger.LogInformation("Processing message\nMessageId: {0}, MessageType: {1}\nTodo-Id: {2}, Todo-Title: {3}, Todo-Description: {4}, Todo-Completed: {5}", messageId, messageType, recieved.Id, recieved.Title, recieved.Description, recieved.Completed);

            // Complete the message, this current message will be dequeued from the queue.
            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
        }
    }
}
