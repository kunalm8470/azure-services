using Azure.Messaging.ServiceBus;
using SharedLibs.Models;

namespace Consumer
{
    public class SessionMessagesWorker : BackgroundService
    {
        private readonly ILogger<SessionMessagesWorker> _logger;
        private readonly ServiceBusSessionProcessor _processor;

        public SessionMessagesWorker(
            ILogger<SessionMessagesWorker> logger,
            ServiceBusSessionProcessor processor
        )
        {
            _logger = logger;
            _processor = processor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);

            _processor.ProcessMessageAsync += ProcessSessionMessageAsync;
            _processor.ProcessErrorAsync += ProcessSessionExceptionAsync;

            await _processor.StartProcessingAsync(stoppingToken).ConfigureAwait(false);
        }

        Task ProcessSessionExceptionAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Error: {0}", args.Exception.Message);
            return Task.CompletedTask;
        }

        async Task ProcessSessionMessageAsync(ProcessSessionMessageEventArgs args)
        {
            string messageId = args.Message.MessageId;
            string sessionId = args.Message.SessionId;
            Todo received = args.Message.Body.ToObjectFromJson<Todo>();

            _logger.LogInformation("MessageId: {0}, SessionId: {1}\nTodo id: {2}, Todo title: {3}, Todo completed: {4}", messageId, sessionId, received.Id, received.Title, received.Completed);
            await args.CompleteMessageAsync(args.Message);
        }
    }
}