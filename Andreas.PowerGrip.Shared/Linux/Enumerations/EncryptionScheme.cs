namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

public enum EncryptionScheme
{
    [EnumMember(Value = "luks1")] Luks1,
    [EnumMember(Value = "luks2")] Luks2,
    Unknown,
}