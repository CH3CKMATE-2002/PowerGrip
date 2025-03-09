namespace Andreas.PowerGrip.Server.Data;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    SuperUserSettings superUserSettings) : DbContext(options)
{
    private readonly SuperUserSettings _superUser = superUserSettings;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //! This was moved into the WebAppExtensions static class
        // AddSuperUser(builder);
    }

    [Obsolete("Do not use; Moved into an extension method of WebApplication")]
    protected void AddSuperUser(ModelBuilder builder)
    {
        var (hash, salt) = HashUtils.HashPassword(_superUser.Password);

        builder.Entity<AppUser>()
            .HasData(new AppUser
            {
                Username = _superUser.Username,
                FullName = _superUser.SystemUsername,
                SystemUsername = _superUser.SystemUsername,
                EmailAddress = _superUser.EmailAddress,
                PasswordHash = hash,
                PasswordSalt = salt,
                BirthDate = _superUser.BirthDate,
            });
    }

    protected void EnsureAppUserModel(ModelBuilder builder)
    {
        builder.Entity<AppUser>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }

    public DbSet<AppUser> Users => Set<AppUser>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<HandshakeRecord> Handshakes => Set<HandshakeRecord>();

    public DbSet<StoredKey> StoredKeys => Set<StoredKey>();
}