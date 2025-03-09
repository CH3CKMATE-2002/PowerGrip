namespace Andreas.PowerGrip.Server.Extensions;

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

    public static WebApplicationBuilder ConfigureConventions(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsProduction())
        {
            builder.Services.Configure<MvcOptions>(options =>
            {
                options.Conventions.Add(new RemoveControllerConvention());
            });
        }
        return builder;
    }
}
