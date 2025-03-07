namespace Andreas.PowerGrip.Server.Settings;

public class SuperUserSettings
{
    public bool EnsureCreation { get; set; } = true;

    public bool PasswordLogin { get; set; } = true;
    
    public string Username { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string SystemUsername { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public bool UseEnvironmentForPassword { get; set; } = true;

    public string PasswordEnvironmentVariableName { get; set; } = "POWERGRIP_SU_PASSWORD";

    public string Password { get; set; } = string.Empty;

    public UserGender Gender { get; set; } = UserGender.Unknown;

    public DateTime? BirthDate { get; set; } = null;
}