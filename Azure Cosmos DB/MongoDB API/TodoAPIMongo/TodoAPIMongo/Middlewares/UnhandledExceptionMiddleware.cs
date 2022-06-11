using Newtonsoft.Json;
using System.Net;
using TodoAPI.Domain.Exceptions;

namespace TodoAPIMongo.Middlewares
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

            if (exception is ResourceNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = "Resource not found";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                StatusCode = (int)statusCode,
                Message = message
            }, Formatting.Indented)).ConfigureAwait(false);
        }
    }
}
