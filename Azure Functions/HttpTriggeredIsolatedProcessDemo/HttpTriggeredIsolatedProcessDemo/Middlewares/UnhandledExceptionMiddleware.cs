using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;

namespace HttpTriggeredIsolatedProcessDemo.Middlewares
{
    public class UnhandledExceptionMiddleware : IFunctionsWorkerMiddleware
    {
        private ILogger<UnhandledExceptionMiddleware> _logger;

        public UnhandledExceptionMiddleware(ILogger<UnhandledExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled exception occured on - {0}\r\n{1}", DateTime.UtcNow, ex);
            }
        }
    }
}
