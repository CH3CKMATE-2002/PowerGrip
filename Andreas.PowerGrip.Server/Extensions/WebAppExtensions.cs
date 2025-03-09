namespace Andreas.PowerGrip.Server.Extensions;

public static class WebAppExtensions
{
    public static WebApplication SeedData(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var settings = scope.ServiceProvider.GetRequiredService<SuperUserSettings>();
            
            if (settings.EnsureCreation)
            {
                SeedSuperUser(context, settings);
            }
            // NOTE: Add any seed data here.
            DropHandshakes(context);
        }

        return app;
    }

    private static void DropHandshakes(AppDbContext context)
    {
        using var transaction = context.Database.BeginTransaction();
        try
        {
            context.Handshakes.RemoveRange(context.Handshakes);
            context.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
        }
    }

    private static void SeedSuperUser(AppDbContext context, SuperUserSettings settings)
    {
        if (!context.Users.Any(u => u.Username == settings.Username))
        {
            var password = settings.Password;

            if (settings.UseEnvironmentForPassword)
            {
                password = Environment.GetEnvironmentVariable(
                    settings.PasswordEnvironmentVariableName);

                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new InvalidConfigurationException(
                        "You didn't define a password for the super user.");
                }
            }

            var (hash, salt) = HashUtils.HashPassword(password);

            var superUser = new AppUser
            {
                Id = Guid.NewGuid(),
                Username = settings.Username,
                FullName = settings.FullName,
                SystemUsername = settings.SystemUsername,
                EmailAddress = settings.EmailAddress,
                PasswordHash = hash,
                PasswordSalt = salt,
                BirthDate = settings.BirthDate,
                CreationDate = DateTime.UtcNow,
                Roles = [ AppRoles.SuperUser, AppRoles.Admin ]
            };

            using var transaction = context.Database.BeginTransaction();
            try
            {
                context.Users.Add(superUser);
                context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public static WebApplication UsePowerGripMiddlewares(this WebApplication app)
    {
        // app.UseMiddleware<IpBanMiddleware>();  // Went obsolete!
        return app;
    }
}