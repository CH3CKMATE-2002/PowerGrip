using UDisks2.DBus;

namespace Andreas.PowerGrip.Shared.Linux.Storage;

/// <summary>
/// Class that represents a block device in Linux systems.
/// </summary>
/// <remarks>
/// The properties are populated on the first time the class is instantiated, if
/// for any reason you need to update them, use the <c>GetXAsync</c> methods not only
/// to get the wanted value, but also to update the underlying <c>X</c> property
/// </remarks> 
public class UnixBlockDevice
{
    private Connection? _bus;

    internal IBlock? BlockProxy;
    internal IPartition? PartitionProxy;
    internal IFilesystem? FileSystemProxy;
    internal IPartitionTable? PartitionTableProxy;

    public string Name { get; private set; } = "unknown";
    public ulong DeviceNumber { get; private set; }
    public BlockUsageKind BlockUsageKind { get; private set; } = BlockUsageKind.Unknown;
    public FileSystemKind FileSystemKind { get; private set; } = FileSystemKind.Unknown;
    public ulong Size { get; private set; }
    public string[] Symlinks { get; private set; } = [];
    public string PartitionName { get; private set; } = string.Empty;
    public ulong PartitionOffsetBytes { get; private set; }
    public uint PartitionNumber { get; private set; }
    public string PartitionType { get; private set; } = string.Empty;
    public Guid PartitionUUID { get; private set; } = Guid.Empty;
    public ulong PartitionFlags { get; private set; }
    public bool IsContainer { get; private set; }
    public bool IsContained { get; private set; }
    public ulong PartitionSizeBytes { get; set; }
    public string[] MountPoints { get; private set; } = [];
    public PartitionTableKind PartitionTableType { get; private set; } = PartitionTableKind.Unknown;
    public string[] PartitionsNames { get; private set; } = [];
    public bool IsReadOnly { get; private set; } = false;
    public string Label { get; private set; } = string.Empty;

    public string HumanReadablePartitionType
        => PartitionTypeHelper.GetHumanReadableType(PartitionTableType, PartitionType);

    public bool IsSuperBlock =>
        HasPartitionTable ||  // It has a partition table -> Definitely a super-block
        (!IsPartition && HasDirectFileSystem); // No partition + Direct filesystem -> Still a super-block

    public bool IsPartition => PartitionProxy is not null;

    public bool HasDirectFileSystem => FileSystemProxy is not null;

    public bool HasPartitionTable => PartitionTableProxy is not null;

    private UnixBlockDevice() { }

    internal static async Task<UnixBlockDevice> CreateAsync(Connection bus, ObjectPath path)
    {
        var device = new UnixBlockDevice();
        device.Name = Path.GetFileName(path.ToString());

        device._bus = bus;

        device.BlockProxy = await device.TryGetProxyAsync<IBlock>(path);
        device.EnsureBlock();

        device.PartitionProxy = await device.TryGetProxyAsync<IPartition>(path);
        device.FileSystemProxy = await device.TryGetProxyAsync<IFilesystem>(path);
        device.PartitionTableProxy = await device.TryGetProxyAsync<IPartitionTable>(path);

        return device;
    }

    private async Task<T?> TryGetProxyAsync<T>(ObjectPath path) where T : class, IDBusObject
    {
        try
        {
            var proxy = _bus!.CreateProxy<T>(DBusKnown.UDisks2Name, path);

            // At least call one of the proxy functions to know if it
            // exists or not!
            switch (proxy)
            {
                case IBlock block:
                    var blockProps = await block.GetAllAsync();
                    Label = blockProps.IdLabel;
                    DeviceNumber = blockProps.DeviceNumber;
                    BlockUsageKind = EnumUtils.ParseEnumMemberValue<BlockUsageKind>(
                        blockProps.IdUsage, BlockUsageKind.Unknown);
                    FileSystemKind = EnumUtils.ParseEnumMemberValue<FileSystemKind>(
                        blockProps.IdType, FileSystemKind.Unknown);
                    Size = blockProps.Size;
                    Symlinks = blockProps.Symlinks.Select(Encoding.UTF8.GetString).ToArray();
                    IsReadOnly = blockProps.ReadOnly;
                    break;

                case IPartition partition:
                    var partProps = await partition.GetAllAsync();
                    PartitionName = partProps.Name;
                    PartitionOffsetBytes = partProps.Offset;
                    PartitionSizeBytes = partProps.Size;
                    PartitionNumber = partProps.Number;
                    PartitionType = partProps.Type;
                    PartitionUUID = Guid.Parse(partProps.UUID);
                    PartitionFlags = partProps.Flags;
                    IsContainer = partProps.IsContainer;
                    IsContained = partProps.IsContained;
                    break;

                case IFilesystem filesystem:
                    var fsProps = await filesystem.GetAllAsync();
                    MountPoints = fsProps.MountPoints.Select(Encoding.UTF8.GetString).ToArray();
                    break;

                case IPartitionTable table:
                    var tableProps = await table.GetAllAsync();
                    PartitionTableType = EnumUtils.ParseEnumMemberValue<PartitionTableKind>(
                        tableProps.Type, PartitionTableKind.Unknown);
                    PartitionsNames = tableProps.Partitions.Select(p => Path.GetFileName(p.ToString())).ToArray();
                    break;

                default:
                    return proxy;
            }

            return proxy;
        }
        catch (DBusException)
        {
            return null;
        }
    }

    #region Block

    public async Task<ulong> GetDeviceNumberAsync()
    {
        EnsureBlock();

        var num = await BlockProxy!.GetDeviceNumberAsync();
        DeviceNumber = num;

        return DeviceNumber;
    }

    public async Task<BlockUsageKind> GetUsageKindAsync()
    {
        EnsureBlock();

        var key = await BlockProxy!.GetIdUsageAsync();
        BlockUsageKind = EnumUtils.ParseEnumMemberValue<BlockUsageKind>(
            key, BlockUsageKind.Unknown);

        return BlockUsageKind;
    }

    public async Task<FileSystemKind> GetFileSystemKind()
    {
        EnsureBlock();

        var key = await BlockProxy!.GetIdTypeAsync();
        FileSystemKind = EnumUtils.ParseEnumMemberValue<FileSystemKind>(
            key, FileSystemKind.Unknown);

        return FileSystemKind;
    }

    public async Task<ulong> GetSizeAsync()
    {
        EnsureBlock();

        Size = await BlockProxy!.GetSizeAsync();
        return Size;
    }

    public async Task<string[]> GetSymbolicLinksAsync()
    {
        EnsureBlock();

        var symlinks = await BlockProxy!.GetSymlinksAsync();
        return symlinks.Select(Encoding.UTF8.GetString).ToArray();
    }

    public async Task<bool> IsReadonlyAsync()
    {
        EnsureBlock();

        var onlyRead = await BlockProxy!.GetReadOnlyAsync();
        return onlyRead;
    }

    public async Task<string> GetLabelAsync()
    {
        EnsureBlock();

        var label = await BlockProxy!.GetIdLabelAsync();
        return label;
    }

    public async Task FormatAsync(FileSystemKind kind, BlockFormatOptions? options = null)
    {
        EnsureBlock();

        options ??= new BlockFormatOptions();
        await BlockProxy!.FormatAsync(kind.GetEnumMemberValue(), options.ToDictionary());
    }

    public async Task FormatAsync(PartitionTableKind kind, BlockFormatOptions? options = null)
    {
        EnsureBlock();

        options ??= new BlockFormatOptions();
        await BlockProxy!.FormatAsync(kind.GetEnumMemberValue(), options.ToDictionary());
    }

    public async Task<UnixDrive> GetPhysicalDriveAsync()
    {
        EnsureBlock();

        var path = await BlockProxy.GetDriveAsync();
        return await UnixDrive.CreateAsync(_bus!, path);
    }

    #endregion

    #region File System

    public async Task<string?> MountAsync(MountOptions? options = null)
    {
        EnsureFileSystem();

        options ??= new MountOptions();
        return await FileSystemProxy!.MountAsync(options.ToDictionary());
    }

    public async Task<bool> UnmountAsync(bool force = false)
    {
        EnsureFileSystem();
        try
        {
            await FileSystemProxy!.UnmountAsync(new Dictionary<string, object>()
            {
                { "force", force }
            });
            return true;
        }
        catch (DBusException e) when (e.ErrorName == "org.freedesktop.UDisks2.Error.DeviceBusy")
        {
            return false;
        }
    }

    public async Task<bool> RepairFileSystemAsync()
    {
        EnsureFileSystem();

        var repaired = await FileSystemProxy!.RepairAsync(new Dictionary<string, object>());

        return repaired;
    }

    public async Task<bool> FileSystemIsConsistentAsync()
    {
        EnsureFileSystem();

        var consistent = await FileSystemProxy!.CheckAsync(new Dictionary<string, object>());

        return consistent;
    }

    public async Task ResizeFileSystemAsync(ulong size)
    {
        EnsureFileSystem();
        // NOTE: As per documentation, the dict is unused
        await FileSystemProxy!.ResizeAsync(size, new Dictionary<string, object>());
    }

    public async Task SetFileSystemLabelAsync(string label)
    {
        EnsureFileSystem();
        // NOTE: As per documentation, the dict is unused
        await FileSystemProxy!.SetLabelAsync(label, new Dictionary<string, object>());
    }

    public async Task SetFileSystemUUIDAsync(Guid uuid)
    {
        EnsureFileSystem();
        // NOTE: As per documentation, the dict is unused
        await FileSystemProxy!.SetUUIDAsync(uuid.ToString(), new Dictionary<string, object>());
    }

    public async Task TakeFileSystemOwnershipAsync(bool recursive = false)
    {
        EnsureFileSystem();
        await FileSystemProxy!.TakeOwnershipAsync(new Dictionary<string, object>()
        {
            { "recursive", recursive }
        });
    }

    public async Task<string[]> GetMountPointsAsync()
    {
        EnsureFileSystem();

        var mounts = await FileSystemProxy!.GetMountPointsAsync();
        MountPoints = mounts.Select(Encoding.UTF8.GetString).ToArray();

        return MountPoints;
    }

    #endregion

    #region Partition

    public async Task DeletePartitionAsync()
    {
        EnsurePartition();

        await PartitionProxy!.DeleteAsync(new Dictionary<string, object>());
    }

    public async Task ResizePartitionAsync(ulong size)
    {
        EnsurePartition();

        await PartitionProxy!.ResizeAsync(size, new Dictionary<string, object>());
    }

    public async Task SetPartitionNameAsync(string name)
    {
        EnsurePartition();

        await PartitionProxy!.SetNameAsync(name, new Dictionary<string, object>());
        PartitionName = name;
    }

    public async Task SetPartitionUUIDAsync(Guid uuid)
    {
        EnsurePartition();

        await PartitionProxy!.SetUUIDAsync(uuid.ToString(), new Dictionary<string, object>());

        PartitionUUID = uuid;
    }

    public async Task SetPartitionTypeAsync(string type)
    {
        EnsurePartition();

        if (PartitionTableType == PartitionTableKind.GuidPartitionTable && !Guid.TryParse(type, out _))
            throw new ArgumentException("Invalid GUID for GPT partition.");
        
        if (PartitionTableType == PartitionTableKind.MasterBootRecord && !type.StartsWith("0x"))
            throw new ArgumentException("MBR type must be a hex code (e.g., 0x83).");

        await PartitionProxy!.SetTypeAsync(type, new Dictionary<string, object>());
        PartitionType = type;
    }

    #endregion

    #region Partition Table

    public async Task<string> CreatePartitionAsync(
        ulong offset,
        ulong size = 0,
        Guid? type = null,
        string name = "",
        CreatePartitionOptions? options = null)
    {
        EnsurePartitionTable();

        options ??= new CreatePartitionOptions();

        var objPath = await PartitionTableProxy!.CreatePartitionAsync(
            Offset: offset,
            Size: size,
            Type: type is null ? string.Empty : type.Value.ToString(),
            Name: name,
            Options: options.ToDictionary()
        );

        return Path.GetFileName(objPath.ToString());
    }

    #endregion

    #region Guards

    private void EnsureBlock()
    {
        if (BlockProxy is null) throw new InvalidOperationException("Block device not found.");
    }

    private void EnsureFileSystem()
    {
        if (FileSystemProxy is null) throw new InvalidOperationException("Block device has no direct file system access.");
    }

    private void EnsurePartition()
    {
        if (PartitionProxy is null) throw new InvalidOperationException("Block device is not a partition.");
    }

    private void EnsurePartitionTable()
    {
        if (PartitionTableProxy is null) throw new InvalidOperationException("Block device has no partition table.");
    }

    #endregion
}
