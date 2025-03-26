namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

public enum DosPartitionType
{
    [EnumMember(Value = "primary")] Primary,
    [EnumMember(Value = "extended")] Extended,
    [EnumMember(Value = "logical")] Logical,
    [EnumMember(Value = "unknown")] Unknown,
}