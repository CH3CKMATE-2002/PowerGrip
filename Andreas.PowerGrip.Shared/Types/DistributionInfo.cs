namespace Andreas.PowerGrip.Shared.Types;

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

    public string Id { get; set; }

    public string IdLike { get; set; }

    public string PrettyName { get; set; }

    public string Name { get; set; }

    public string Version { get; set; }

    public string Codename { get; set; }

    public string? DebianVersion { get; set; }

    public DistributionInfo()
    {
        var dict = EnvLoader.Load("/etc/os-release");

        PrettyName = dict.GetValueOrDefault("PRETTY_NAME", "Unknown installed distribution");
        Name = dict.GetValueOrDefault("NAME", "Unknown");
        Id = dict.GetValueOrDefault("ID", "unknown");
        IdLike = dict.GetValueOrDefault("ID_LIKE", "unknown");
        Version = dict.GetValueOrDefault("VERSION_ID", "1.0");
        Codename = dict.GetValueOrDefault("VERSION_CODENAME", "unknown");

        if (File.Exists("/etc/debian_version"))
        {
            DebianVersion = File.ReadAllText("/etc/debian_version");
        }
    }
}