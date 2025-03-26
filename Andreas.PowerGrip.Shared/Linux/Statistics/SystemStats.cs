namespace Andreas.PowerGrip.Shared.Linux.Statistics;

public class SystemStats
{
    #region Low Level Syscalls & Structs
    [DllImport("libc")]
    private static extern int sysinfo(out Sysinfo info);

    [StructLayout(LayoutKind.Sequential)]
    private struct Sysinfo
    {
        public long Uptime;
        public ulong Load1, Load5, Load15;
        public ulong TotalRam, FreeRam, SharedRam, BufferRam;
        public ulong TotalSwap, FreeSwap;
        public ushort Procs;
        public ulong TotalHigh, FreeHigh;
        public uint MemUnit;
    }
    #endregion

    public TimeSpan Uptime { get; private set; }
    public double LoadAverage1Minute { get; private set; }
    public double LoadAverage5Minute { get; private set; }
    public double LoadAverage15Minute { get; private set; }
    
    // Memory values in bytes (calculated using MemUnit)
    public ulong TotalRam { get; private set; }
    public ulong AvailableRam { get; private set; }
    public ulong SharedRam { get; private set; }
    public ulong BufferedRam { get; private set; }
    public ulong UsedRam => TotalRam - TrueAvailableRam;
    public ulong TrueAvailableRam => AvailableRam + BufferedRam + SharedRam;
    
    public ulong TotalSwapSpace { get; private set; }
    public ulong AvailableSwapSpace { get; private set; }
    public ulong UsedSwapSpace => TotalSwapSpace - AvailableSwapSpace;
    
    public ushort ProcessesCount { get; private set; }
    public ulong TotalHighMemory { get; private set; }
    public ulong AvailableHighMemory { get; private set; }
    public uint MemoryUnitBytes { get; private set; }

    private SystemStats() { }

    public static SystemStats QueryCurrentStats()
    {
        if (sysinfo(out var info) == 0)
        {
            return BuildFromSysinfoStruct(info);
        }
        throw new InvalidOperationException("Could not read system information.");
    }

    private static SystemStats BuildFromSysinfoStruct(Sysinfo sysinfo)
    {
        var memUnit = (ulong)sysinfo.MemUnit;

        return new SystemStats
        {
            Uptime = TimeSpan.FromSeconds(sysinfo.Uptime),
            LoadAverage1Minute = sysinfo.Load1 / 65536.0,
            LoadAverage5Minute = sysinfo.Load5 / 65536.0,
            LoadAverage15Minute = sysinfo.Load15 / 65536.0,
            
            // Convert memory values using MemUnit
            TotalRam = sysinfo.TotalRam * memUnit,
            // TrueAvailableRam = (sysinfo.FreeRam + sysinfo.BufferRam) * memUnit,
            AvailableRam = sysinfo.FreeRam * memUnit,
            SharedRam = sysinfo.SharedRam * memUnit,
            BufferedRam = sysinfo.BufferRam * memUnit,
            
            TotalSwapSpace = sysinfo.TotalSwap * memUnit,
            AvailableSwapSpace = sysinfo.FreeSwap * memUnit,
            
            ProcessesCount = sysinfo.Procs,
            
            TotalHighMemory = sysinfo.TotalHigh * memUnit,
            AvailableHighMemory = sysinfo.FreeHigh * memUnit,
            MemoryUnitBytes = sysinfo.MemUnit
        };
    }
}
