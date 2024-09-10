using Newtonsoft.Json;
using System.Net;

namespace GeneralEntries.Global;

public class GlobalExceptionHandler : IMiddleware
{
    private ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
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

    private const string JsonContentType = "application/json";
    private const int InternalServerErrorStatusCode = (int)HttpStatusCode.InternalServerError;

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        string message = ex.Message.ToString();
        context.Response.ContentType = JsonContentType;
        context.Response.StatusCode = InternalServerErrorStatusCode;

        _logger.LogError($"Exception Details {message}");

        var response = new Error()
        {
            StatusCode = context.Response.StatusCode,
            Message = message
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}