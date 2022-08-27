using BlobContainers.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BlobContainers.API.Middlewares;

public class UnhandledExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<UnhandledExceptionMiddleware> _logger;

    public UnhandledExceptionMiddleware(
        RequestDelegate next,
        IWebHostEnvironment env,
        ILogger<UnhandledExceptionMiddleware> logger
    )
    {
        _next = next;
        _env = env;
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
            await HandleUnhandledExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleUnhandledExceptionAsync(HttpContext context, Exception ex)
    {
        string traceId = Guid.NewGuid().ToString();

        _logger.LogError("Unhandled exception occured on - {currentDateTime}\r\nTraceId: {traceId}, {exceptionMessage}", DateTime.UtcNow, traceId, ex.ToString());

        int statusCode = (int)HttpStatusCode.InternalServerError;

        string message = ex.Message;

        switch (ex)
        {
            case InvalidRequestException:
                message = string.Join(Environment.NewLine, (ex as InvalidRequestException)?.Errors);
                statusCode = (int)HttpStatusCode.BadRequest;
                break;
        }

        ProblemDetails details = new()
        {
            Status = statusCode,
            Type = $"https://httpstatuses.com/{statusCode}",
            Title = (_env.EnvironmentName == "Development")
                    ? message
                    : "Something went wrong. Please try after some time",
            Detail = (_env.EnvironmentName == "Development")
                     ? ex.ToString()
                     : "Something went wrong. Please try after some time",
            Instance = traceId
        };

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(details));
    }
}