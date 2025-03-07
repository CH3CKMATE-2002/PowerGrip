namespace Andreas.PowerGrip.Shared.Types.Auth;

public class AppJwtTokens
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public TimeSpan AccessLifetime { get; set; } = TimeSpan.Zero;

    public TimeSpan RefreshLifetime { get; set; } = TimeSpan.Zero;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}