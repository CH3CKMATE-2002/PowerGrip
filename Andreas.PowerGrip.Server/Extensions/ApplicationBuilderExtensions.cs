namespace Andreas.PowerGrip.Server.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseFrontEnd(this IApplicationBuilder app)
    {
        var settings = app.ApplicationServices.GetRequiredService<FrontEndSettings>();

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = settings?.Path ?? "frontend";
        });

        return app;
    }

    public static IApplicationBuilder UseLocalHostCors(this IApplicationBuilder app)
    {
        app.UseCors("AllowAllLocalHostApps");
        return app;
    }
}