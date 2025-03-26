namespace Andreas.PowerGrip.Shared.Utilities;

public static class FileUtils
{
    public static bool SetPermissions(string path, UnixFileMode mode)
    {
        Guard.ThrowIfNotLinux($"{nameof(SetPermissions)} only works on Linux");

        try
        {
            File.SetUnixFileMode(path, mode);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }
}