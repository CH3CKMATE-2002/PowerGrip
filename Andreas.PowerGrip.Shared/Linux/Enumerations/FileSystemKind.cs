namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

public enum FileSystemKind
{
    /// <summary>The <c>Ext2</c> Filesystem</summary>
    [EnumMember(Value = "ext2")] Extended2,
    /// <summary>The <c>Ext3</c> Filesystem</summary>
    [EnumMember(Value = "ext3")] Extended3,
    /// <summary>The <c>Ext4</c> Filesystem</summary>
    [EnumMember(Value = "ext4")] Extended4,
    /// <summary>The <c>xfs</c> Filesystem</summary>
    [EnumMember(Value = "xfs")] XFileSystem,
    /// <summary>The <c>vfat</c> Filesystem</summary>
    [EnumMember(Value = "vfat")] VirtualFileAllocationTable,
    /// <summary>The <c>ntfs</c> Filesystem (New Technology Filesystem)</summary>
    [EnumMember(Value = "ntfs")] NewTechnologyFileSystem,
    /// <summary>The <c>f2fs</c> Filesystem</summary>
    [EnumMember(Value = "f2fs")] FlashFriendlyFileSystem,
    /// <summary>The <c>nilfs2</c> Filesystem</summary>
    [EnumMember(Value = "nilfs2")] Nil2,
    /// <summary>The <c>ExtFat</c> Filesystem</summary>
    [EnumMember(Value = "exfat")] ExFat,
    /// <summary>The <c>btrfs</c> Filesystem (B-Tree Filesystem)</summary>
    [EnumMember(Value = "btrfs")] BTree,
    /// <summary>The <c>udf</c> Filesystem</summary>
    [EnumMember(Value = "udf")] UniversalDiskFormat,
    /// <summary>The filesystem used for swap files and swap partitions</summary>
    [EnumMember(Value = "swap")] Swap,
    /// <summary>The <c>reiserfs</c> Filesystem</summary>
    [EnumMember(Value = "reiserfs")] Reiser,
    /// <summary>The <c>jfs</c> Filesystem (Journaled Filesystem)</summary>
    [EnumMember(Value = "jfs")] Journaled,
    /// <summary>The <c>hfsplus</c> Filesystem</summary>
    [EnumMember(Value = "hfsplus")] HierarchicalPlus,
    /// <summary>The <c>ISO9660</c> Filesystem</summary>
    [EnumMember(Value = "iso9660")] Iso9660Standard,
    /// <summary>The <c>ocfs2</c> Filesystem</summary>
    [EnumMember(Value = "ocfs2")] OracleCluster,
    /// <summary>The <c>gfs2</c> Filesystem</summary>
    [EnumMember(Value = "gfs2")] Global2,
    /// <summary>The <c>minix</c> Filesystem</summary>
    [EnumMember(Value = "minix")] Minix,
    /// <summary>The <c>cramfs</c> Filesystem</summary>
    [EnumMember(Value = "cramfs")] CompressedRom,
    /// <summary>The <c>erofs</c> Filesystem</summary>
    [EnumMember(Value = "erofs")] EnhancedReadOnly,
    /// <summary>The filesystem used in apple devices</summary>
    [EnumMember(Value = "apfs")] Apple,
    /// <summary>The <c>zfs</c> Filesystem</summary>
    [EnumMember(Value = "zfs")] Zettabyte,
    /// <summary>The <c>crypto_LUKS</c> Encrypted Filesystem</summary>
    [EnumMember(Value = "crypto_LUKS")] CryptoLinuxUnifiedKeySetup,
    /// <summary>No file system</summary>
    [EnumMember(Value = "empty")] Empty,
    /// <summary>Unknown file system</summary>
    [EnumMember(Value = "unknown")] Unknown
}