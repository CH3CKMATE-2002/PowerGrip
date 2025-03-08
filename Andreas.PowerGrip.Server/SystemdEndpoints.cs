namespace Andreas.PowerGrip.Server;

public static class SystemdEndpoints
{
    public const string SystemServices = "/system";

    public const string DebugServices = "/debug";
    
    public const string AmIRoot = $"{DebugServices}/am-i-root";

    public const string AptUpdate = $"{SystemServices}/apt-update";
}