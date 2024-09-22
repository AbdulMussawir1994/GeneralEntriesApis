using Serilog;
using Serilog.Formatting.Json;

namespace GeneralEntries.Helpers.Data_Structure;

public static class AppExtensionLog
{
    public static void SerilogConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.WriteTo.Console();
            loggerConfig.WriteTo.File(new JsonFormatter(),"Logs/applogs.txt", rollingInterval: RollingInterval.Day);
        });
    }

}
