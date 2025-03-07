namespace Andreas.PowerGrip.Server.Models;

public class AppUser
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();

    public string Username { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string SystemUsername { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string PasswordSalt { get; set; } = string.Empty;

    public UserGender Gender { get; set; } = UserGender.Unknown;

    public DateTime? BirthDate { get; set; } = null;

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    public List<string> Roles { get; set; } = [];

    [JsonIgnore] // Don't send that to the end-user.
    public List<RefreshToken> RefreshTokens { get; set; } = [];

    /// <summary>
    /// Fetches the base claims of the <see cref="AppUser"/> with
    /// no composite claims (user claims without roles).
    /// </summary>
    /// <returns>Returns <see cref="List{Claim}" /> of the user without the roles.</returns>
    public List<Claim> GetBaseClaims()
    {
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, Id.ToString()),
            new(ClaimTypes.Name, Username),
            new(AppClaimTypes.FullName, FullName),
            new(ClaimTypes.Gender, Gender.ToString()),
            new(ClaimTypes.DateOfBirth, BirthDate?.ToString() ?? ""),
            new(AppClaimTypes.SystemUsername, SystemUsername ?? string.Empty),
        ];

        return claims;
    }

    public static bool IsValidUsername(string username)
    {
        return !username.IsWhiteSpace() &&
            username.All(c => c.IsLetter() || c.IsDigit() || c.IsSymbol());
    }
}