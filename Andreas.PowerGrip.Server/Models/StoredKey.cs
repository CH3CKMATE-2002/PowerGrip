namespace Andreas.PowerGrip.Server.Models;

public class StoredKey
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string KeyFilePath = string.Empty;

    [ForeignKey("User.Id")]
    public Guid UserId { get; set; }

    public AppUser User { get; set; } = null!;
}