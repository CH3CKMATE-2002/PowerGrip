namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

public enum BlockUsageKind
{
    [EnumMember(Value = "filesystem")] Filesystem,   // A normal formatted partition
    [EnumMember(Value = "raid")] RaidMember,   // Part of a RAID array
    [EnumMember(Value = "lvm2")] LvmVolume,    // Part of an LVM (Logical Volume Manager)
    [EnumMember(Value = "crypto")] Encrypted,    // Encrypted volume (e.g., LUKS)
    [EnumMember(Value = "other")] Unrecognized, // A valid DBus value, but unclear usage
    [EnumMember(Value = "unknown")] Unknown       // DBus returned an unexpected value
}