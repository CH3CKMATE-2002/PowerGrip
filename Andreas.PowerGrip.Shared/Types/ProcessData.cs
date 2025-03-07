namespace Andreas.PowerGrip.Shared.Types;

public class ProcessData
{
    public bool LaunchSuccess { get; set; } = true;

    public string Tag { get; set; } = string.Empty;
    
    public int ExitCode { get; set; } = 0;

    public string StdError { get; set; } = string.Empty;

    public string StdOutput { get; set; } = string.Empty;

    public TimeSpan RunningTime { get; set; } = TimeSpan.Zero;

    public static ProcessData FailedToLaunch(string? processName = null, string reason = "")
    {
        var name = string.IsNullOrEmpty(processName) ? "process" : processName;
        var tag = $"Failed to run {name}";

        return new ProcessData
        {
            LaunchSuccess = false,
            Tag = tag + (string.IsNullOrEmpty(reason) ? "" : $": {reason}")
        };
    }
}