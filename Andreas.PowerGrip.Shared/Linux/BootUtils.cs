namespace Andreas.PowerGrip.Shared.Linux;

public static class BootUtils
{
    [DllImport("libsystemd.so.0")]
    private static extern int sd_id128_get_boot(out SdId128 bootId);

    [StructLayout(LayoutKind.Sequential)]
    private struct SdId128
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] bytes;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(32);
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));  // Hexadecimal with double digits (e.g. 0A)
            return sb.ToString();
        }
    }

    public static Guid GetBootId()
    {
        sd_id128_get_boot(out var bootId);
        return Guid.Parse(bootId.ToString());
    }
}