using System.Net;
using System.Text.Json;
using TableStorageDemo.Domain.Exceptions;

namespace TableStorageDemo.API.Middlewares
{
    public class UnhandledExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UnhandledExceptionMiddleware> _logger;

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
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occured on - {when}\r\n{exception}", DateTime.UtcNow, ex);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Internal Server Error";

            if (exception is ResourceNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = "Resource not found";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                status_code = (int)statusCode,
                message = message
            }, new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
}
