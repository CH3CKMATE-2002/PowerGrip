namespace Andreas.PowerGrip.Shared.Utilities;

public static class AesUtils
{
    public static (string Key, string IV) GenerateAesKey()
    {
        using var aes = Aes.Create();
        aes.GenerateKey();
        aes.GenerateIV();
        return (Convert.ToBase64String(aes.Key), Convert.ToBase64String(aes.IV));
    }

    public static string Encrypt(string plaintext, string key, string iv)
    {
        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(key);
        aes.IV = Convert.FromBase64String(iv);

        using var encryptor = aes.CreateEncryptor();
        byte[] encryptedBytes = encryptor.TransformFinalBlock(
            Encoding.UTF8.GetBytes(plaintext), 0, plaintext.Length
        );

        return Convert.ToBase64String(encryptedBytes);
    }

    public static string Decrypt(string encryptedData, string key, string iv)
    {
        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(key);
        aes.IV = Convert.FromBase64String(iv);

        using var decryptor = aes.CreateDecryptor();
        byte[] decryptedBytes = decryptor.TransformFinalBlock(
            Convert.FromBase64String(encryptedData), 0, Convert.FromBase64String(encryptedData).Length
        );

        return Encoding.UTF8.GetString(decryptedBytes);
    }
}
