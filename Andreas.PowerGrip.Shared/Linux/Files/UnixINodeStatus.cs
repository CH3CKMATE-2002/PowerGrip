using Andreas.PowerGrip.Shared.Linux.Users;
using Microsoft.VisualBasic;

namespace Andreas.PowerGrip.Shared.Linux.Files;

public class UnixINodeStatus
{
    #region Low Level Syscalls & Structs

    [DllImport("libc", SetLastError = true, EntryPoint = "stat")]
    private static extern int stat(string path, out Stat buf);

    [StructLayout(LayoutKind.Sequential)]
    private struct Timespec
    {
        public long tv_sec;
        public long tv_nsec;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Stat
    {
        public ulong st_dev;         // 0
        public ulong st_ino;         // 8
        public ulong st_nlink;       // 16  👈 Wait, WHAT? Yes, it's before st_mode!
        public uint st_mode;         // 24
        public uint st_uid;          // 28
        public uint st_gid;          // 32
        public int __pad0;           // 36  👈 Padding (not in man page! Yes... FUCK!)
        public ulong st_rdev;        // 40
        public long st_size;         // 48
        public long st_blksize;      // 56
        public long st_blocks;       // 64
        public Timespec st_atim;     // 72
        public Timespec st_mtim;     // 88
        public Timespec st_ctim;     // 104
        private long __unused1;      // 120  👈 Reserved garbage for glibc, because why not?
        private long __unused2;      // 128
        private long __unused3;      // 136
    }

    #endregion

    public ulong DeviceId { get; private set; }
    public ulong INodeNumber { get; private set; }
    public UnixFileMode Mode { get; private set; }
    public ulong HardLinkCount { get; private set; }
    public uint OwnerId { get; private set; }
    public uint OwningGroupId { get; private set; }
    public ulong SpecialDeviceId { get; private set; }
    public long TotalSizeBytes { get; private set; }
    public long BlockSize { get; private set; }
    public long BlockCount { get; private set; }
    public DateTimeOffset LastAccessTime { get; private set; }
    public DateTimeOffset LastModificationTime { get; private set; }
    public DateTimeOffset LastChangeTime { get; private set; }

    private UnixINodeStatus() { }

    public static UnixINodeStatus Get(string path)
    {
        int result = stat(path, out Stat status);
        if (result != 0)
        {
            int errno = Marshal.GetLastWin32Error();
            throw new IOException($"Failed to stat '{path}'. Error: {errno}");
        }
        return BuildFromStatStruct(status);
    }

    private static UnixINodeStatus BuildFromStatStruct(Stat status)
    {
        var fileStatus = new UnixINodeStatus
        {
            DeviceId = status.st_dev,
            INodeNumber = status.st_ino,
            Mode = (UnixFileMode)status.st_mode,
            HardLinkCount = status.st_nlink,
            OwnerId = status.st_uid,
            OwningGroupId = status.st_gid,
            SpecialDeviceId = status.st_rdev,
            TotalSizeBytes = status.st_size,
            BlockSize = status.st_blksize,
            BlockCount = status.st_blocks,
            LastAccessTime = ConvertTimespec(status.st_atim),
            LastModificationTime = ConvertTimespec(status.st_mtim),
            LastChangeTime = ConvertTimespec(status.st_ctim),
        };

        return fileStatus;
    }

    private static DateTimeOffset ConvertTimespec(Timespec ts)
    {
        return DateTimeOffset.FromUnixTimeSeconds(ts.tv_sec)
            .AddTicks(ts.tv_nsec / 100);
    }
}