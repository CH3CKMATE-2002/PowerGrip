namespace Andreas.PowerGrip.Shared.Extensions;

public static class UnixFileModeExtensions
{
    public static uint ToUInt32(this UnixFileMode mode)
        => Convert.ToUInt32(mode);
}