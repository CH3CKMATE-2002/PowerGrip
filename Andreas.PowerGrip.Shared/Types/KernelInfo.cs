namespace Andreas.PowerGrip.Shared.Types;

public class KernelInfo
{
    public string Name { get; set; } = "Unknown Name";

    public string Release { get; set; } = "1.0";

    public string Version { get; set; } = "1.0";

    public MachineType MachineType { get; set; } = MachineType.Unknown;

    public string Hostname { get; set; } = "Unknown";

    public string DomainName { get; set; } = "";
}