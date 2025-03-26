using System.Runtime.Serialization;

namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

/// <summary>
/// Represents the active state of a systemd unit.
/// </summary>
public enum UnitActiveState
{
    /// <summary>
    /// The unit is currently active.
    /// </summary>
    [EnumMember(Value = "active")] Active,

    /// <summary>
    /// The unit is inactive.
    /// </summary>
    [EnumMember(Value = "inactive")] Inactive,

    /// <summary>
    /// The unit is in the process of activating.
    /// </summary>
    [EnumMember(Value = "activating")] Activating,

    /// <summary>
    /// The unit is in the process of deactivating.
    /// </summary>
    [EnumMember(Value = "deactivating")] Deactivating,

    /// <summary>
    /// The unit has failed.
    /// </summary>
    [EnumMember(Value = "failed")] Failed,

    /// <summary>
    /// The active state of the unit is unknown.
    /// </summary>
    [EnumMember(Value = "unknown")] Unknown
}