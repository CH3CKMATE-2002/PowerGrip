namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

public enum LuksKeyDerivation
{
    [EnumMember(Value = "pbkdf2")] Pbkdf2,
    [EnumMember(Value = "argon2i")] Argon2I,
    [EnumMember(Value = "argon2id")] Argon2ID,
    [EnumMember(Value = "none")] None,
}