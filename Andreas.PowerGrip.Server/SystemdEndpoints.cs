namespace Andreas.PowerGrip.Server;

public static class SystemdEndpoints
{
    public const string SystemServices = "/system";
    
    public const string AmIRoot = $"{SystemServices}/i-am-root";
}