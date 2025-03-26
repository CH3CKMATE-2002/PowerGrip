namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

[Flags]
public enum MountingFlags
{
    None = 0,
    ReadWrite = 1 << 0,  // "rw"
    Relatime = 1 << 1,  // "relatime"
    NoExec = 1 << 2,  // "noexec"
    NoSuid = 1 << 3,  // "nosuid"
}