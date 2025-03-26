namespace Andreas.PowerGrip.Shared.Linux;

public class DistributionInfo
{
    private static readonly string[] PossibleFiles = 
    {
        "/etc/os-release",       // Modern, widely adopted
        // TODO: Maybe let the lesser ones contribute?
        "/etc/lsb-release",      // Debian/Ubuntu-based
        "/etc/debian_version",   // Legacy Debian-based
        "/etc/redhat-release"    // RHEL-based
    };

    public string Id { get; set; } = null!;

    public string IdLike { get; set; } = null!;

    public string PrettyName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Version { get; set; } = null!;

    public string Codename { get; set; } = null!;

    public string? DebianVersion { get; set; } = null!;

    private DistributionInfo() { }

    public static DistributionInfo GetInfo()
    {
        var info = new DistributionInfo();

        var dict = EnvLoader.Load("/etc/os-release");

        info.PrettyName = dict.GetValueOrDefault("PRETTY_NAME", "Unknown installed distribution");
        info.Name = dict.GetValueOrDefault("NAME", "Unknown");
        info.Id = dict.GetValueOrDefault("ID", "unknown");
        info.IdLike = dict.GetValueOrDefault("ID_LIKE", "unknown");
        info.Version = dict.GetValueOrDefault("VERSION_ID", "1.0");
        info.Codename = dict.GetValueOrDefault("VERSION_CODENAME", "unknown");

        if (File.Exists("/etc/debian_version"))
        {
            info.DebianVersion = File.ReadAllText("/etc/debian_version");
        }

        return info;
    }
}