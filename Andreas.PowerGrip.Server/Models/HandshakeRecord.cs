namespace Andreas.PowerGrip.Server.Models;

public class HandshakeRecord
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public IPAddress FromAddress { get; set; } = IPAddress.None;

    public string RsaPublicKey { get; set; } = string.Empty;

    public string RsaPrivateKey { get; set; } = string.Empty;

    public string? AesKey { get; set; } = null;

    public int Attempts { get; set; } = 0;

    public DateTime? BannedAt { get; set; } = null;

    public bool IsBanned(TimeSpan banTime)
    {
        if (BannedAt is null) return false;

        var thresh = BannedAt.Value.Add(banTime);

        return thresh >= DateTime.UtcNow;
    }
}