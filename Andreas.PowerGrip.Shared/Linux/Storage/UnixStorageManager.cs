using UDisks2.DBus;

namespace Andreas.PowerGrip.Shared.Linux.Storage;

public class UnixStorageManager : IDisposable
{
    private Connection _bus = new(Address.System);
    private IObjectManager? _disks;
    private IManager? _manager;

    public string UDisks2Version { get; private set; } = string.Empty;

    public FileSystemKind[] SupportedFileSystems { get; private set; } = [];

    public EncryptionScheme[] SupportedEncryptionTypes { get; private set; } = [];

    public EncryptionScheme DefaultEncryptionType { get; private set; } = EncryptionScheme.Unknown;

    private UnixStorageManager() {}

    public static async Task<UnixStorageManager> CreateAsync()
    {
        var storage = new UnixStorageManager();

        await storage._bus.ConnectAsync();

        var managerPath = Path.Join(DBusKnown.UDisks2Path, "Manager");

        storage._disks = storage._bus.CreateProxy<IObjectManager>(DBusKnown.UDisks2Name, DBusKnown.UDisks2Path);
        storage._manager = storage._bus.CreateProxy<IManager>(DBusKnown.UDisks2Name, managerPath);

        var props = await storage._manager.GetAllAsync();
        storage.SupportedFileSystems = props.SupportedFilesystems
            .Select(fs => EnumUtils.ParseEnumMemberValue<FileSystemKind>(fs))
            .ToArray();

        storage.SupportedEncryptionTypes = props.SupportedEncryptionTypes
            .Select(e => EnumUtils.ParseEnumMemberValue<EncryptionScheme>(e))
            .ToArray();

        storage.DefaultEncryptionType = EnumUtils.ParseEnumMemberValue<EncryptionScheme>(props.DefaultEncryptionType);

        storage.UDisks2Version = props.Version;

        return storage;
    }

    public async Task<List<UnixBlockDevice>> GetBlockDevicesAsync()
    {
        var blockDevicePaths = await _manager!.GetBlockDevicesAsync(new Dictionary<string, object>());
        var devices = new List<UnixBlockDevice>();

        foreach (var path in blockDevicePaths)
        {
            devices.Add(await UnixBlockDevice.CreateAsync(_bus, path));
        }

        return devices;
    }

    public async Task<ManagerReport> CanRepairAsync(FileSystemKind type)
    {
        var key = type.ToString();

        if (string.IsNullOrEmpty(key)) return new ManagerReport();

        var (available, required) = await _manager!.CanRepairAsync(key);

        return new ManagerReport
        {
            HasAbility = available,
            Required = required,
        };
    }

    public async Task<ManagerReport> CanCheckErrorsAsync(FileSystemKind type)
    {
        var key = type.ToString();

        if (string.IsNullOrEmpty(key)) return new ManagerReport();

        var (available, required) = await _manager!.CanCheckAsync(key);

        return new ManagerReport
        {
            HasAbility = available,
            Required = required,
        };
    }

    public async Task<ManagerReport> CanFormatAsync(FileSystemKind type)
    {
        var key = type.ToString();

        if (string.IsNullOrEmpty(key)) return new ManagerReport();

        var (available, required) = await _manager!.CanFormatAsync(key);

        return new ManagerReport
        {
            HasAbility = available,
            Required = required,
        };
    }

    public async Task<ManagerReport> CanResizeAsync(FileSystemKind type)
    {
        var key = type.ToString();

        if (string.IsNullOrEmpty(key)) return new ManagerReport();

        var (available, flags, required) = await _manager!.CanResizeAsync(key);

        return new ManagerReport
        {
            HasAbility = available,
            Required = required,
            ResizeFlags = (StorageResizeFlags)flags,
        };
    }

    public async Task<UnixBlockDevice?> GetBlockAsync(string name)
    {
        try
        {
            var path = Path.Join(DBusKnown.UDisks2Path, "block_devices", name);
            return await UnixBlockDevice.CreateAsync(_bus, path);
        }
        catch (DBusException)
        {
            return null;
        }
    }

    public void Dispose()
    {
        _bus.Dispose();
    }
}