using Azure.Messaging.ServiceBus;
using SharedLibs.Models;

namespace Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ServiceBusProcessor _processor;

        public Worker(
            ILogger<Worker> logger,
            ServiceBusProcessor processor
        )
        {
            _logger = logger;
            _processor = processor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);

            _processor.ProcessMessageAsync += ProcessMessageAsync;
            _processor.ProcessErrorAsync += ProcessSessionExceptionAsync;

            await _processor.StartProcessingAsync(stoppingToken).ConfigureAwait(false);
        }

        Task ProcessSessionExceptionAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Error: {0}", args.Exception.Message);
            return Task.CompletedTask;
        }

        async Task ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            string messageId = args.Message.MessageId;
            Todo received = args.Message.Body.ToObjectFromJson<Todo>();

            _logger.LogInformation("MessageId: {0}, \nTodo id: {1}, Todo title: {2}, Todo completed: {3}", messageId, received.Id, received.Title, received.Completed);
            await args.CompleteMessageAsync(args.Message);
        }
    }
}