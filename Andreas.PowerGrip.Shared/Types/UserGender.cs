namespace Andreas.PowerGrip.Shared.Types;

/// <summary>
/// An enum that represents the user's gender.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserGender
{
    /// <summary>
    /// The user prefers not to speak.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The user is male.
    /// </summary>
    Male = 1,

    /// <summary>
    /// The user is female.
    /// </summary>
    Female = 2,

    /// <summary>
    /// The user is non-binary.
    /// </summary>
    NonBinary = 3,
}