using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace GeneralEntries.Global;

public class GlobalExceptionHandler : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Type = "https://httpstatuses.com/500",
            Title = "An unexpected error occurred!",
            Detail = _env.IsDevelopment() ? exception.StackTrace : "An internal server error occurred. Please contact support."
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        _logger.LogError(exception, "Unhandled exception occurred");

        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}