namespace Andreas.PowerGrip.Server.Extensions;

public static class ModelMappers
{
    public static AppUser MapRequest(this CreateUserRequest request)
    {
        var (hash, salt) = HashUtils.HashPassword(request.Password);

        return new()
        {
            Username = request.Username,
            FullName = request.FullName,
            EmailAddress = request.EmailAddress.ToLowerInvariant().Trim(),
            SystemUsername = request.SystemUsername,
            Roles = request.Roles.ToList(),
            BirthDate = request.BirthDate,
            Gender = request.Gender,
            PasswordHash = hash,
            PasswordSalt = salt,
        };
    }
}