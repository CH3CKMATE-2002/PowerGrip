namespace Andreas.PowerGrip.Server.Models;

public class UserKeys
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// It is safer to store the path instead of the key file
    /// </summary>
    public string KeyFilePath = string.Empty;

    [ForeignKey("User.Id")]
    public Guid UserId { get; set; }

    public AppUser User { get; set; } = null!;
}