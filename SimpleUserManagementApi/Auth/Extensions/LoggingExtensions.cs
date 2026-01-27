using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Templates;

namespace SimpleUserManagementApi.Auth.Extensions;

public static class LoggingExtensions
{
    public static void UseSerilogLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(outputTemplate: 
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}")
            .WriteTo.File("logs/log.jsonl", rollingInterval: RollingInterval.Day)
            .Enrich.FromLogContext()
            .CreateLogger();
        
        builder.Host.UseSerilog();
        Log.Debug("started logging");
    }
}