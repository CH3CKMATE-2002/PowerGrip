using System.Runtime.Serialization;

namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

/// <summary>
/// Represents the load state of a systemd unit.
/// </summary>
public enum UnitLoadState
{
    /// <summary>
    /// The unit has been successfully loaded.
    /// </summary>
    [EnumMember(Value = "loaded")] Loaded,

    /// <summary>
    /// The unit file was not found.
    /// </summary>
    [EnumMember(Value = "not-found")] NotFound,

    /// <summary>
    /// The unit file exists but is in a bad state.
    /// </summary>
    [EnumMember(Value = "bad")] Bad,

    /// <summary>
    /// The unit encountered an error during loading.
    /// </summary>
    [EnumMember(Value = "error")] Error,

    /// <summary>
    /// The load state of the unit is unknown.
    /// </summary>
    Unknown
}