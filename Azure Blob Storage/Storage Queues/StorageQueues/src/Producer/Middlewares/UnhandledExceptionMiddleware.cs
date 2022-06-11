using System.Net;
using System.Text.Json;

namespace Producer.Middlewares
{
    public class UnhandledExceptionMiddleware
    {
        private RequestDelegate _next;
        private ILogger<UnhandledExceptionMiddleware> _logger;

        public UnhandledExceptionMiddleware(RequestDelegate next,
            ILogger<UnhandledExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled exception occured on - {0}\r\n{1}", DateTime.UtcNow, ex);
                await HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Internal Server Error";

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                StatusCode = (int)statusCode,
                Message = message
            })).ConfigureAwait(false);
        }
    }
}
