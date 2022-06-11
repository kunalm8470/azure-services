namespace Producer.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string CORRELATIONID_HEADER = "x-correlation-id";

        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;
        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                string correlationId = Guid.NewGuid().ToString();

                if (!context.Request.Headers.ContainsKey(CORRELATIONID_HEADER))
                {
                    context.Request.Headers.Add(CORRELATIONID_HEADER, correlationId);
                }

                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Add(CORRELATIONID_HEADER, correlationId);

                    return Task.CompletedTask;
                });

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed due to - {errorMessage}", ex.Message);
            }
        }
    }
}
