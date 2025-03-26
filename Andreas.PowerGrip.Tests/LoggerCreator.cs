using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Andreas.PowerGrip.Tests;

public static class LoggerCreator
{
    public static ILogger<T> Create<T>()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Code,
                outputTemplate: "[{Timestamp:HH:mm:ss.fff} - {Level:u4}]: {Message:lj}{NewLine}{Exception}",
                applyThemeToRedirectedOutput: !Console.IsOutputRedirected // 👈 THIS IS THE MAGIC THAT FORCES `dotnet test` to output colors!
            )
            .WriteTo.File(
                "logs/test-logs-.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} - {Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .CreateLogger();

        var factory = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog(Log.Logger);
        });

        return factory.CreateLogger<T>();
    }
}