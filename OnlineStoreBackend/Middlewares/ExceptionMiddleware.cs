using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlineStoreBackend.Api.Models;

namespace OnlineStoreBackend.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError($"Unhandled exception: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var result = new InternalExceptionResponse
        {
            ErrorMessage = exception.Message,
            Stacktrace = exception.StackTrace
        };

        switch (exception)
        {
            case TaskCanceledException:
            {
                context.Response.StatusCode = 400;
                break;
            }
            default:
            {
                context.Response.StatusCode = 500;
                _logger.LogError(exception, "Unhandled exception");
                break;
            }
        }

        // ugly but works -_-
        var jsonResult = JsonSerializer.Serialize(result);
        await context.Response.WriteAsync(jsonResult);
    }
}