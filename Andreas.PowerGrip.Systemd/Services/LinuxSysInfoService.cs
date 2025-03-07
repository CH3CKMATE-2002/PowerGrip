namespace Andreas.PowerGrip.Systemd.Services;

public class LinuxSysInfoService : ISysInfoService
{
    public const string SystemCpuInfoPath = "/sys/cpuinfo";
    public const string SystemMemInfoPath = "/sys/meminfo";

    private readonly DistributionInfo distribution = new DistributionInfo();

    public string GetCodename()
    {
        return distribution.Codename;
    }

    public CpuInfo[] GetProcessorsInfo()
    {
        return CpuInfo.ParseFromFile(SystemCpuInfoPath);
    }

    public string GetDesktopEnvironment()
        => Environment.GetEnvironmentVariable("XDG_SESSION_DESKTOP") ?? "TTY";

    public string GetHostname()
    {
        // return Environment.GetEnvironmentVariable("HOST") ?? "unknown";
        return UnixUtils.GetHostname();
    }

    public KernelInfo GetKernelInfo()
    {
        return UnixUtils.GetKernelInfo();
    }

    public string GetOsName()
    {
        return distribution.PrettyName;
    }

    public string GetOsVersion()
    {
        return distribution.Version;
    }

    public TimeSpan GetUptime()
    {
        return UnixUtils.GetUptime();
    }

    public MemoryInfo GetMemoryInfo()
    {
        return MemoryInfo.ParseFromFile(SystemMemInfoPath);
    }

    public PowerSupplyInfo GetPowerSupplyInfo()
    {
        return new PowerSupplyInfo();
    }
}