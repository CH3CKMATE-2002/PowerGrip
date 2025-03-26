namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

[Flags]
public enum StorageResizeFlags : ulong
{
    None = 0,
    OfflineShrink = 2,    // BD_FS_OFFLINE_SHRINK
    OfflineGrow = 4,      // BD_FS_OFFLINE_GROW
    OnlineShrink = 8,     // BD_FS_ONLINE_SHRINK
    OnlineGrow = 16       // BD_FS_ONLINE_GROW
}