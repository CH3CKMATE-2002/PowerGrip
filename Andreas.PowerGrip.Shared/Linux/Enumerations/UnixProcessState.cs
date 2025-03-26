namespace Andreas.PowerGrip.Shared.Linux.Enumerations;


/// <summary>
/// Represents the state of a Unix process as reported in the /proc filesystem.
/// Each value corresponds to a state character you typically see in /proc/[pid]/stat.
/// </summary>
public enum UnixProcessState
{
    /// <summary>
    /// Not a known state. This should be used when the state is unknown.
    /// </summary>
    Unknown,

    /// <summary>
    /// Running (R): The process is currently executing on the CPU.
    /// </summary>
    Running,

    /// <summary>
    /// Sleeping (S): The process is in an interruptible sleep, waiting for an event or signal.
    /// </summary>
    Sleeping,

    /// <summary>
    /// Uninterruptible Sleep (D): The process is waiting in an uninterruptible sleep, usually for I/O.
    /// </summary>
    UninterruptibleSleep,

    /// <summary>
    /// Stopped (T): The process has been stopped, typically due to a job control signal.
    /// </summary>
    Stopped,

    /// <summary>
    /// Tracing Stop (t): The process is stopped while being traced (e.g., by a debugger).
    /// </summary>
    TracingStop,

    /// <summary>
    /// Zombie (Z): The process has terminated, but its parent has not yet retrieved its exit status.
    /// </summary>
    Zombie,

    /// <summary>
    /// Dead (X): The process is dead. This state is rarely seen and indicates the process is finished.
    /// </summary>
    Dead,

    /// <summary>
    /// Wakekill (K): The process is being awakened to be killed. This is a rarely used state.
    /// </summary>
    WakeKill,

    /// <summary>
    /// Waking (W): The process is in the process of waking up from a sleep state. This is also rarely seen.
    /// </summary>
    Waking,

    /// <summary>
    /// Idle (I): The process is doing nothing at the moment.
    /// </summary>
    Idle,
}