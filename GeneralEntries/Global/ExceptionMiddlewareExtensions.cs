using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace GeneralEntries.Global
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, NLog.Logger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var errorMessage = contextFeature.Error.InnerException != null && !string.IsNullOrEmpty(contextFeature.Error.InnerException.Message)
                         ? contextFeature.Error.InnerException.Message
                         : contextFeature.Error.Message;

                        logger.Error($"Something went wrong in {contextFeature.Endpoint?.DisplayName}: {errorMessage}");
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            context.Response.StatusCode,
                            Message = "Internal Error. " + errorMessage
                        }));
                    }
                });
            });
        }
    }
}
