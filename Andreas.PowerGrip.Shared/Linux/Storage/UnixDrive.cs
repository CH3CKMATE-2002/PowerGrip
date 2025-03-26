using UDisks2.DBus;

namespace Andreas.PowerGrip.Shared.Linux.Storage;

public class UnixDrive
{
    private IDrive? _proxy;

    public string Id { get; private set; } = string.Empty;
    public string Serial { get; private set; } = string.Empty;
    public string WorldWideName { get; set; } = "Unknown";
    public bool CanPowerOff { get; private set; } = false;
    public bool Ejectable { get; private set; } = false;
    public string Model { get; private set; } = string.Empty;
    public bool Removable { get; private set; }
    public string Revision { get; private set; } = string.Empty;
    public bool OpticalDiskInserted { get; private set; }
    public bool OpticalDiskIsBlank { get; private set; }
    public string Vendor { get; private set; } = string.Empty;
    public bool HasAvailableMedia { get; private set; }
    public MediaKind[] CompatibleMediaTypes { get; private set; } = [];
    public DateTimeOffset TimeDetected { get; private set; }
    public bool CanRemoveMedia { get; private set; }
    public int RotationRate { get; private set; }  // Measured in RPM.
    public bool IsRotationRateUnknown => RotationRate == -1;
    public bool IsRotating => RotationRate != 0;
    public bool IsNonRotating => RotationRate == 0;
    public string CurrentMedia { get; private set; } = string.Empty;
    public ulong Size { get; private set; }
    public ConnectionBusType ConnectionBus { get; private set; }

    private UnixDrive() { }

    internal static async Task<UnixDrive> CreateAsync(Connection bus, ObjectPath path)
    {
        var drive = new UnixDrive();
        drive._proxy = bus.CreateProxy<IDrive>(DBusKnown.UDisks2Name, path);
        await drive.PopulateProps();

        return drive;
    }

    private async Task PopulateProps()
    {
        var props = await _proxy!.GetAllAsync();

        Id = props.Id;
        Serial = props.Serial;
        CanPowerOff = props.CanPowerOff;
        Ejectable = props.Ejectable;
        Model = props.Model;
        Removable = props.Removable;
        Revision = props.Revision;
        OpticalDiskInserted = props.Optical;
        OpticalDiskIsBlank = props.OpticalBlank;
        Vendor = props.Vendor;
        Size = props.Size;
        HasAvailableMedia = props.MediaAvailable;
        WorldWideName = props.WWN;

        CompatibleMediaTypes = props.MediaCompatibility
            .Select(m => EnumUtils.ParseEnumMemberValue<MediaKind>(m, MediaKind.Unknown))
            .ToArray();
        
        long secondsOffset = (long)(props.TimeDetected / 1000000);
        TimeDetected = DateTimeOffset.FromUnixTimeSeconds(secondsOffset);
        
        CanRemoveMedia = props.MediaRemovable;
        RotationRate = props.RotationRate;
        CurrentMedia = props.Media;

        ConnectionBus = EnumUtils.ParseEnumMemberValue<ConnectionBusType>(
            props.ConnectionBus, ConnectionBusType.Unknown);
    }

    public Task EjectAsync()
    {
        if (!Ejectable)
            throw new InvalidOperationException("Cannot eject a non-ejectable drive");
        // NOTE: As per documentation, the dict is unused
        return _proxy!.EjectAsync(new Dictionary<string, object>());
    }

    public Task PowerOffAsync()
    {
        if (!CanPowerOff)
            throw new InvalidOperationException("Cannot power-off drive");
        // NOTE: As per documentation, the dict is unused
        return _proxy!.PowerOffAsync(new Dictionary<string, object>());
    }
}