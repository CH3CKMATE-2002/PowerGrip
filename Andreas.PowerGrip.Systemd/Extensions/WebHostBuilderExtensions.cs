namespace Andreas.PowerGrip.Systemd.Extensions;

public static class WebHostBuilderExtensions
{
    public static IWebHostBuilder ConfigureAsSystemdService(this IWebHostBuilder webHostBuilder, IConfiguration config)
    {
        var udsSettings = config.GetSection(nameof(UdsOptions)).Get<UdsOptions>();

        udsSettings ??= new UdsOptions();  // Use the default values.
        var socketPath = udsSettings.SocketFile;

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