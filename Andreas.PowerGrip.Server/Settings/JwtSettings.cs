namespace Andreas.PowerGrip.Server.Settings;

public class JwtSettings
{
    public bool UseEnvironmentForKey { get; set; } = true;

    public string SecretKey { get; set; } = string.Empty;

    public string KeyEnvironmentVariableName { get; set; } = "POWERGRIP_JWT_SECRET";

    public string ValidIssuer { get; set; } = string.Empty;

    public string ValidAudience { get; set; } = string.Empty;

    public TimeSpan Lifetime { get; set; } = TimeSpan.Zero;

    public TimeSpan RefreshLifetime { get; set; } = TimeSpan.FromDays(7);

    public SymmetricSecurityKey ResolveSigningKey()
    {
        var key = UseEnvironmentForKey
            ? Environment.GetEnvironmentVariable(KeyEnvironmentVariableName)
            : SecretKey;
        
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidConfigurationException(
                "JWT Secret Key is missing! Check your environment variables or appsettings.json.");
        }

        if (key.Length < 32)
        {
            throw new InvalidConfigurationException(
                "JWT Secret Key must be at least 32 characters long.");
        }

        var keyBytes = Encoding.UTF8.GetBytes(key);
        var signingKey = new SymmetricSecurityKey(keyBytes);

        return signingKey;
    }
}