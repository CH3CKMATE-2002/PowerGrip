namespace Andreas.PowerGrip.Systemd.Extensions;

public static class WebAppBuilderExtensions
{
    public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration) // Load from appsettings.json
            .CreateLogger();
        
        builder.Host.UseSerilog();

        return builder;
    }
}