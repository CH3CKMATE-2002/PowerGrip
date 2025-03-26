using systemd1.DBus;

namespace Andreas.Test.LinuxProcesses.Utilities.Services;

internal static class SystemdUtils
{
    public static string NormalizeServiceName(string serviceName)
        => serviceName.EndsWith(".service") ? serviceName : $"{serviceName}.service";

    public static string NormalizeUnitName(string unitName)
    {
        string[] unitTypes =
        [
            ".service", ".target", ".socket", ".swap",
            ".mount",   ".timer",  ".device", ".path"
        ];

        return unitTypes.Any(unitName.EndsWith) ? unitName : $"{unitName}.service";
    }
}