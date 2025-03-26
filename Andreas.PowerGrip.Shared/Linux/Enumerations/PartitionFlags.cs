namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

[Flags]
public enum PartitionFlags : ulong
{
    SystemPartition    = 0,
    DosBootable        = 1 << 6,
    LegacyBiosBootable = 1 << 1,
    ReadOnly           = 1 << 59,
    Hidden             = 1 << 61,
    NoAutoMount        = 1 << 62 
}