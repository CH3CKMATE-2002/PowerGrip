namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

/// <summary>
/// Represents the service's current status on the system.
/// </summary>
public enum StatusSummary
{
    /// <summary>
    /// The service exists but is not currently running.
    /// </summary>
    Stopped,

    /// <summary>
    /// The service exists and is currently running.
    /// </summary>
    Running,

    /// <summary>
    /// The service exists but is disabled and not running.
    /// </summary>
    Disabled,

    /// <summary>
    /// An unknown service state, which should not normally occur.
    /// </summary>
    Unknown
}