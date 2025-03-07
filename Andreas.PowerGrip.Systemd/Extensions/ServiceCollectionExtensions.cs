namespace Andreas.PowerGrip.Systemd.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDaemonConfigurations(this IServiceCollection services, IConfiguration config)
    {
        var udsSettings = config.GetSection(nameof(UdsSocketOptions)).Get<UdsSocketOptions>();
        services.AddSingleton(udsSettings ?? new UdsSocketOptions());

        return services;
    }

    public static IServiceCollection AddDaemonServices(this IServiceCollection services)
    {
        services.AddHostedService<OnStartupService>();
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            services.AddSingleton<ISysInfoService, LinuxSysInfoService>();
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
        
        return services;
    }

    public static IServiceCollection AddLocalHostCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllLocalHostApps", policy =>
            {
                policy.SetIsOriginAllowed(origin =>
                {
                    var uri = new Uri(origin);
                    return uri.Host.ToLower() is "localhost" or "127.0.0.1";
                }).AllowAnyHeader().AllowAnyMethod();
            });
        });

        return services;
    }

    public static IServiceCollection AddAndConfigureSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            // TODO: Configure API definitions here
        });

        return services;
    }
}