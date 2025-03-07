namespace Andreas.PowerGrip.Shared.Utilities;

public static class RsaUtils
{
    public static (string PublicKey, string PrivateKey) GenerateKeys()
    {
        using var rsa = RSA.Create(4096); // 4096-bit RSA for strong security

        return (
            Convert.ToBase64String(rsa.ExportRSAPublicKey()),
            Convert.ToBase64String(rsa.ExportRSAPrivateKey())
        );
    }

    public static string Encrypt(string data, string publicKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);

        var encryptedData = rsa.Encrypt(
            System.Text.Encoding.UTF8.GetBytes(data),
            RSAEncryptionPadding.OaepSHA256
        );

        return Convert.ToBase64String(encryptedData);
    }

    public static string Decrypt(string encryptedData, string privateKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

        var decryptedData = rsa.Decrypt(
            Convert.FromBase64String(encryptedData),
            RSAEncryptionPadding.OaepSHA256
        );

        return Encoding.UTF8.GetString(decryptedData);
    }
}
