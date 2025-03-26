namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

public enum PartitionTableKind
{
    [EnumMember(Value = "gpt")] GuidPartitionTable,
    [EnumMember(Value = "dos")] MasterBootRecord,
    [EnumMember(Value = "unknown")] Unknown,
}