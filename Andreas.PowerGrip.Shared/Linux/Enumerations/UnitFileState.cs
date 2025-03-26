using System.Runtime.Serialization;

namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

/// <summary>
/// Represents the state of a systemd unit file.
/// </summary>
public enum UnitFileState
{
    /// <summary>
    /// The unit file is enabled and will start automatically.
    /// </summary>
    [EnumMember(Value = "enabled")] Enabled,

    /// <summary>
    /// The unit file is disabled and will not start automatically.
    /// </summary>
    [EnumMember(Value = "disabled")] Disabled,

    /// <summary>
    /// The unit file is static, meaning it is not enabled but required for dependencies.
    /// </summary>
    [EnumMember(Value = "static")] Static,

    /// <summary>
    /// The unit file is masked, preventing it from being started.
    /// </summary>
    [EnumMember(Value = "masked")] Masked,

    /// <summary>
    /// The unit file is aliased to another.
    /// </summary>
    [EnumMember(Value = "alias")] Alias,

    /// <summary>
    /// The state of the unit file is unknown.
    /// </summary>
    [EnumMember(Value = "unknown")] Unknown
}
