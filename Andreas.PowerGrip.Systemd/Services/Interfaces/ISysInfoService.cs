namespace Andreas.PowerGrip.Systemd.Services.Interfaces;

public interface ISysInfoService
{
    string GetOsName();

    string GetOsVersion();

    string GetCodename();

    string GetDesktopEnvironment();

    string GetHostname();

    CpuInfo[] GetProcessorsInfo();

    MemoryInfo GetMemoryInfo();

    TimeSpan GetUptime();

    KernelInfo GetKernelInfo();

    PowerSupplyInfo GetPowerSupplyInfo();
}