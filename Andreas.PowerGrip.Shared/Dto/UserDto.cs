namespace Andreas.PowerGrip.Shared.Dto;

public class UserDto
{
    public string Username { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string SystemUsername { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public UserGender Gender { get; set; } = UserGender.Unknown;

    public DateTime? BirthDate { get; set; } = null;
}