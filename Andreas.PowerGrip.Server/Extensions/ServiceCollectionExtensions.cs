namespace Andreas.PowerGrip.Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPowerGripConfigurations(this IServiceCollection services, IConfiguration config)
    {
        // if I want an object of 'IOption<JwtSettings>' to be added to the service collection.
        // services.Configure<JwtSettings>(config.GetSection(nameof(JwtSettings)));
        
        // But the better way:
        var jwtSettings = config.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
        services.AddSingleton(Guard.ReturnOrThrowIfNull(jwtSettings));

        var frontEndSettings = config.GetSection(nameof(FrontEndSettings)).Get<FrontEndSettings>();
        services.AddSingleton(Guard.ReturnOrThrowIfNull(frontEndSettings));

        var superUserSettings = config.GetSection(nameof(SuperUserSettings)).Get<SuperUserSettings>();
        services.AddSingleton(Guard.ReturnOrThrowIfNull(superUserSettings));

        var databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
        services.AddSingleton(Guard.ReturnOrThrowIfNull(databaseSettings));

        var udsOptions = config.GetSection(nameof(UdsOptions)).Get<UdsOptions>();
        services.AddSingleton(udsOptions ?? new UdsOptions());

        return services;
    }

    public static IServiceCollection AddFrontEnd(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(nameof(FrontEndSettings)).Get<FrontEndSettings>();

        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = settings?.Path ?? "frontend"; // Path to the frontend
        });

        return services;
    }

    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration config)
    {
        var dbSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

        if (dbSettings is null)
        {
            throw new InvalidConfigurationException(
                "You must define valid database settings in your configuration.");
        }
        
        services.AddDbContext<AppDbContext>(options =>
            {
                switch (dbSettings.Provider)
                {
                    // case DatabaseProvider.SqlServer:
                    //     options.UseSqlServer(dbSettings.ConnectionString);
                    //     break;
                    case DatabaseProvider.Sqlite:
                    default:
                        options.UseSqlite(dbSettings.ConnectionString);
                        break;
                }
            });

        return services;
    }

    public static IServiceCollection AddPowerGripServices(this IServiceCollection services)
    {
        // services.AddScoped<IThingy, Thingy>();
        services.AddSingleton<IJwtProvider, JwtProvider>();
        // services.AddSingleton<ISystemService, SystemService>();  // TODO: This implementation of ISystemService is obsolete!
        services.AddSingleton<UdsHttpClient>();
        services.AddScoped<IHandshakeService, HandshakeService>();
        services.AddScoped<IAppUserManager, AppUserManager>();  // Scoped, as it uses a scoped service "AppDbContext"
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

    public static IServiceCollection AddPowerGripJwtAuth(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(nameof(JwtSettings)).Get<JwtSettings>() ??
            throw new InvalidConfigurationException(
                "You must define JwtSettings in appsettings.json, please refer to PowerGrip's documentation");
        
        var key = settings.ResolveSigningKey();
        
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Issuer:
                ValidateIssuer = true,
                ValidIssuer = settings.ValidIssuer,
                
                // Audience:
                ValidateAudience = true,
                ValidAudience = settings.ValidAudience,
                
                // Secret Key:
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,

                ValidateTokenReplay = true,
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var response = JsonSerializer.Serialize(new ServiceResponse
                    {
                        Success = false,
                        Title = "Unauthorized Access",
                        Errors =
                        [
                            new ServiceError
                            {
                                Kind = ErrorKind.AuthError,
                                Reason = "You must provide a valid JWT Token."
                            }
                        ]
                    });

                    return context.Response.WriteAsync(response);
                },
                
                OnAuthenticationFailed = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    var errorMessage = context.Exception?.Message ?? "Unknown authentication error.";
                    Log.Warning("JWT Authentication Failed: {Message}", errorMessage);

                    var response = JsonSerializer.Serialize(new ServiceResponse
                    {
                        Success = false,
                        Title = "Unauthenticated Access",
                        Errors =
                        [
                            new ServiceError
                            {
                                Kind = ErrorKind.AuthError,
                                Reason = context.Exception?.Message ?? "Unknown authentication error.",
                            }
                        ]
                    });

                    return context.Response.WriteAsync(response);
                },

                OnForbidden = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";

                    var response = JsonSerializer.Serialize(new ServiceResponse
                    {
                        Success = false,
                        Title = "Forbidden Access",
                        Errors =
                        [
                            new ServiceError
                            {
                                Kind = ErrorKind.NotFoundError,
                                Reason = "You're not allowed to access this resource.",
                            }
                        ]
                    });

                    return context.Response.WriteAsync(response);
                }
            };
        });
        
        services.AddAuthorizationBuilder()
            .AddPolicy(AppPolicies.AllowAdmins, builder =>
            {
                // builder.RequireClaim(ClaimTypes.Role, AppRoles.Admin);
                builder.RequireRole(AppRoles.Admin);
            })
            .AddPolicy(AppPolicies.AllowSuperUsers, builder =>
            {
                builder.RequireRole(AppRoles.SuperUser);
            });

        return services;
    }

    public static IServiceCollection ConfigureCustomValidationError(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errorMessages = context.ModelState
                    .Where(e => e.Value is { Errors.Count: > 0 })
                    .SelectMany(e => e.Value!.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                var response = new ServiceResponse
                {
                    Success = false,
                    Title = "Validation Errors Occurred",
                    Errors = errorMessages.Select(message => new ServiceError
                    {
                        Kind = ErrorKind.ValidationError,
                        Reason = message,
                    }).ToList(),
                };

                return new BadRequestObjectResult(response);
            };
        });

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
}