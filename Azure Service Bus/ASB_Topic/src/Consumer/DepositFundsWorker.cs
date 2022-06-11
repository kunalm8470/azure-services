using Azure.Messaging.ServiceBus;
using SharedLibs.Models;

namespace Consumer
{
    public class DepositFundsWorker : BackgroundService
    {
        private readonly ILogger<DepositFundsWorker> _logger;
        private readonly ServiceBusProcessor _processor;

        public DepositFundsWorker(
            ILogger<DepositFundsWorker> logger,
            ServiceBusProcessor processor
        )
        {
            _logger = logger;
            _processor = processor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting deposit worker....");

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
            DepositFunds recieved = args.Message.Body.ToObjectFromJson<DepositFunds>();

            _logger.LogInformation("Processing message\nMessageId: {0}, MessageType: {1}\nAccount-Id: {2}, Account-Holder-Name: {3}, Transaction-Amount: {4}, Transaction-Id: {5}, Transaction-Date:{6}", messageId, messageType, recieved.AccountId, recieved.Name, recieved.Amount, recieved.TransactionId, recieved.TransactionDate);

            // Complete the message, this current message will be dequeued from the queue.
            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
        }
    }
}
