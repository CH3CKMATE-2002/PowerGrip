namespace Andreas.PowerGrip.Shared.Utilities;

public static class RandomUtils
{
    public static byte[] RandomBytes(int length)
    {
        var bytes = new byte[length];
        RandomNumberGenerator.Fill(bytes);
        return bytes;
    }

    public static string RandomBase64Bytes(int length)
    {
        var bytes = RandomBytes(length);
        return Convert.ToBase64String(bytes).ToUrlSafeString();
    }
}