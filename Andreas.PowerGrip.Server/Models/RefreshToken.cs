namespace Andreas.PowerGrip.Server.Models;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Token { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime ExpiresAt { get; set; }

    public bool Revoked { get; set; } = false;

    public DateTime? RevokedAt { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    [ForeignKey("User.Id")]
    public Guid UserId { get; set; }

    public AppUser User { get; set; } = null!;
}