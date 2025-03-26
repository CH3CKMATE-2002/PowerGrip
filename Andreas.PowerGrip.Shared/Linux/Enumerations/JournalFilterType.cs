namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

public enum JournalFilterType
{
    /// <summary>
    /// Logs from a specific systemd service
    /// </summary>
    [EnumMember(Value = "_SYSTEMD_UNIT")] UnitName,

    /// <summary>
    /// Logs from a specific cgroup (useful for containers)
    /// </summary>
    [EnumMember(Value = "_SYSTEMD_CGROUP")] ContainerGroup,

    /// <summary>
    /// Logs from a specific executable name
    /// </summary>
    [EnumMember(Value = "_COMM")] ExecutableName,

    /// <summary>
    /// Logs from a specific binary path
    /// </summary>
    [EnumMember(Value = "_EXE")] BinaryPath,

    /// <summary>
    /// Logs from a process with specific command-line args
    /// </summary>
    [EnumMember(Value = "_CMDLINE")] Commandline,

    /// <summary>
    /// Logs from a specific process ID
    /// </summary>
    [EnumMember(Value = "_PID")] ProcessId,

    /// <summary>
    /// Logs from a specific user ID
    /// </summary>
    [EnumMember(Value = "_UID")] UserId,

    /// <summary>
    /// Logs from a specific group ID
    /// </summary>
    [EnumMember(Value = "_GID")] GroupId,

    /// <summary>
    /// Logs from a specific user session
    /// </summary>
    [EnumMember(Value = "_AUDIT_SESSION")] UserSession,

    /// <summary>
    /// Filter logs by priority (0-7)
    /// </summary>
    [EnumMember(Value = "_PRIORITY")] Priority,

    /// <summary>
    /// Filter logs within a specific timestamp
    /// </summary>
    [EnumMember(Value = "_SOURCE_REALTIME_TIMESTAMP")] RealTimeTimestamp,

    /// <summary>
    /// Logs from a specific boot session
    /// </summary>
    [EnumMember(Value = "_BOOT_ID")] BootId,

    /// <summary>
    /// Logs from a specific kernel device
    /// </summary>
    [EnumMember(Value = "_KERNEL_DEVICE")] KernelDevice,
    
    /// <summary>
    /// Logs from a specific kernel subsystem
    /// </summary>
    [EnumMember(Value = "_KERNEL_SUBSYSTEM")] KernelSubsystem,

    /// <summary>
    /// Filter logs by message transport type (syslog, audit, journal)
    /// </summary>
    [EnumMember(Value = "_TRANSPORT")] Transport
}