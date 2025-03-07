namespace Andreas.PowerGrip.Systemd.Extensions;

public static class WebHostBuilderExtensions
{
    public static IWebHostBuilder ConfigureAsSystemdService(this IWebHostBuilder webHostBuilder, IConfiguration config)
    {
        var udsSettings = config.GetSection(nameof(UdsSocketOptions)).Get<UdsSocketOptions>();

        udsSettings ??= new UdsSocketOptions();  // Use the default values.
        var socketPath = udsSettings.Path;

        // Delete existing socket to avoid stale permissions
        if (File.Exists(socketPath))
        {
            File.Delete(socketPath);
        }

        webHostBuilder.ConfigureKestrel(serverOptions =>
        {
            serverOptions.ListenUnixSocket(socketPath);
        });

        return webHostBuilder;
    }
}