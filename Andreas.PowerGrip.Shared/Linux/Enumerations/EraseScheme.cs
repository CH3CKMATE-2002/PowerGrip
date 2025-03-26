namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

public enum EraseScheme
{
    [EnumMember(Value = "zero")] Zero,
    [EnumMember(Value = "ata-secure-erase")] SecureErase,
    [EnumMember(Value = "ata-secure-erase-enhanced")] SecureEraseEnhanced,
    [EnumMember(Value = "none")] None,
}