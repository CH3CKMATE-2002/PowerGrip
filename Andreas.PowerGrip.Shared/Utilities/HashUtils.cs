namespace Andreas.PowerGrip.Shared.Utilities;

public static class HashUtils
{
    [Obsolete($"Use {nameof(HashPassword)} instead.")]
    public static (string Hash, string Salt) Sha256HashPassword(string password)
    {
        var passBytes = Encoding.UTF8.GetBytes(password);
        var saltBytes = RandomNumberGenerator.GetBytes(16);

        // Pass the salt! lol...
        var passWithSalt = new byte[passBytes.Length + saltBytes.Length];
        Array.Copy(passBytes, 0, passWithSalt, 0, passBytes.Length);
        Array.Copy(saltBytes, 0, passWithSalt, passBytes.Length, saltBytes.Length);

        var hashBytes = SHA256.HashData(passWithSalt);

        var hashed = Convert.ToBase64String(hashBytes);
        var salt = Convert.ToBase64String(saltBytes);

        return (hashed, salt);
    }

    public static (string Hash, string Salt) HashPassword(string password)
    {
        // Generate a 128-bit salt using a sequence of cryptographically strong random bytes.
        byte[] saltBytes = RandomNumberGenerator.GetBytes(128 / 8);

        // Derive a 256-bit sub-key (use HMAC-SHA256 with 100,000 iterations)
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100_000,
            numBytesRequested: 256 / 8
        ));

        // Convert the salt to Base64 for storage
        string salt = Convert.ToBase64String(saltBytes);

        return (hashed, salt);
    }

    public static bool ComparePasswords(string password, string hashedPassword, string passwordSalt)
    {
        // Get back the salt bytes
        byte[] saltBytes = Convert.FromBase64String(passwordSalt);

        // Hash the password with the given salt bytes
        string rehashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100_000,
            numBytesRequested: 256 / 8
        ));

        // Compare with the previously hashed password
        return rehashed == hashedPassword;
    }
}