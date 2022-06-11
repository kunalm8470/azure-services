namespace WorkerServiceDemo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Start job
            _logger.LogInformation("Starting job");

            // Continue until app is shut down
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTime.Now);

                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    IScopedOperation operation = scope.ServiceProvider.GetService<IScopedOperation>();
                    _logger.LogInformation(operation.OperationId);
                }

                await Task.Delay(2000, stoppingToken).ConfigureAwait(false);
            }

            // Job ends
            _logger.LogInformation("Job ends");
        }
    }
}