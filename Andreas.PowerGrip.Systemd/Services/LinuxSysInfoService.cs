namespace Andreas.PowerGrip.Systemd.Services;

public class LinuxSysInfoService : ISysInfoService
{
    public const string SystemCpuInfoPath = "/sys/cpuinfo";
    public const string SystemMemInfoPath = "/sys/meminfo";

    private readonly DistributionInfo distribution = DistributionInfo.GetInfo();
    private readonly KernelInfo kernel = KernelInfo.GetInfo();
    private readonly PowerSupplyInfo powerSupply = PowerSupplyInfo.GetInfo();

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
        return kernel.HostName;
    }

    public KernelInfo GetKernelInfo()
    {
        return kernel;
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
        var stats = SystemStats.QueryCurrentStats();
        return stats.Uptime;
    }

    public MemoryInfo GetMemoryInfo()
    {
        return MemoryInfo.ParseFromFile(SystemMemInfoPath);
    }

    public PowerSupplyInfo GetPowerSupplyInfo()
    {
        powerSupply.Update();
        return powerSupply;
    }
}