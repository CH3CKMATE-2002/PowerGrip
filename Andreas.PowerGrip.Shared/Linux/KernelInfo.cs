namespace Andreas.PowerGrip.Shared.Linux;


public class KernelInfo
{
    #region Low Level Syscalls & Structs
    [DllImport("libc")]
    private static extern int uname(out Utsname buf);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    private struct Utsname
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)] public string SysName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)] public string NodeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)] public string Release;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)] public string Version;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)] public string Machine;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 65)] public string DomainName;
    }

    #endregion

    // Properties mapped from Utsname struct
    public string SystemName { get; set; } = "unknown";
    public string HostName { get; set; } = "unknown";
    public string Release { get; set; } = "unknown";
    public string Version { get; set; } = "unknown";
    public MachineArchitecture MachineArchitecture { get; private set; }
    public string RawMachineName { get; private set; } = string.Empty;
    public string DomainName { get; set; } = string.Empty;

    private KernelInfo() { }

    public static KernelInfo GetInfo()
    {
        var success = uname(out var utsname) == 0;
        if (success)
        {
            return BuildFromUtsnameStruct(utsname);
        }

        throw new InvalidOperationException("Could not fetch kernel information");
    }

    private static KernelInfo BuildFromUtsnameStruct(Utsname uts)
    {
        return new KernelInfo
        {
            SystemName = uts.SysName,
            HostName = uts.NodeName,
            Release = uts.Release,
            Version = uts.Version,
            MachineArchitecture = ArchitectureUtils.Parse(uts.Machine),
            RawMachineName = uts.Machine,
            DomainName = uts.DomainName,
        };
    }
}