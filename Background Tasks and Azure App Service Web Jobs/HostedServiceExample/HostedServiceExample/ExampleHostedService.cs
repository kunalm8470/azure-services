namespace HostedServiceExample
{
    public class ExampleHostedService : IHostedService
    {
        private readonly ILogger<ExampleHostedService> _logger;

        public ExampleHostedService(ILogger<ExampleHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting background task");

            /*
                Not recommended:
                Infinitely running task, this blocks IHost from starting the server.
            */
            //while (!cancellationToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Doing work on {time}", DateTimeOffset.Now);
            //    await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken).ConfigureAwait(false);
            //}

            /*
                Way 1:

                Not blocking to bootstrap the IHost
                using factory method Task.Run
            */
            Task.Run(() => DoTask(cancellationToken), cancellationToken);

            /*
                Way 2:

                Not blocking to bootstrap the IHost
                using C# 7 discards
            */
            //_ = DoTask(cancellationToken);

            return Task.CompletedTask;
        }

        // Helper method for way 2
        private async Task DoTask(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Doing work on {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken).ConfigureAwait(false);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping background task");

            return Task.CompletedTask;
        }
    }
}
