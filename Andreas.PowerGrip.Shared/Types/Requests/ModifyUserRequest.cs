namespace Andreas.PowerGrip.Shared.Types.Requests;

public class ModifyUserRequest
{
    public string Username { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string SystemUsername { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public UserGender Gender { get; set; } = UserGender.Unknown;

    public string Password { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; } = null;

    public IEnumerable<string> Roles { get; set; } = [];
}