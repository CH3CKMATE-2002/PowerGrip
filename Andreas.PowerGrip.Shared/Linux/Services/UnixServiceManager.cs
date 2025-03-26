using systemd1.DBus;

namespace Andreas.Test.LinuxProcesses.Utilities.Services;

public class UnixServiceManager : IDisposable
{
    private readonly Connection _bus = new(Address.System);
    private readonly IManager _systemd;

    public UnixServiceManager()
    {
        _bus.ConnectAsync().GetAwaiter().GetResult();
        _systemd = _bus.CreateProxy<IManager>(DBusKnown.SystemdName, DBusKnown.SystemdPath);
    }

    public async Task<bool> ServiceExistsAsync(string serviceName)
    {
        serviceName = SystemdUtils.NormalizeServiceName(serviceName);
        try
        {
            string state = await _systemd.GetUnitFileStateAsync(serviceName);
            return state != "invalid";
        }
        catch (DBusException)
        {
            // Handle unexpected errors if necessary
            return false;
        }
    }

    public async Task<bool> ServiceIsLoadedAsync(string serviceName)
    {
        serviceName = SystemdUtils.NormalizeServiceName(serviceName);
        try
        {
            var unitPath = await _systemd.GetUnitAsync(serviceName);  // If this succeeds.
            return true;  // Boom! There you have it!
        }
        catch (DBusException e) when (e.ErrorName == "org.freedesktop.systemd1.NoSuchUnit")
        {
            return false;
        }
    }

    public async Task<UnixUnit?> GetServiceAsync(string serviceName, bool forceLoad = false)
    {
        serviceName = SystemdUtils.NormalizeServiceName(serviceName);

        bool exists = await ServiceExistsAsync(serviceName);

        if (!exists) return null;

        // Create base unit with file state
        var state = await _systemd.GetUnitFileStateAsync(serviceName);
        
        var unit = new UnixUnit
        {
            Name = serviceName,
            FileState = EnumUtils.ParseEnumMemberValue<UnitFileState>(state, UnitFileState.Unknown)
        };

        bool attemptLoad = forceLoad;

        if (!forceLoad) attemptLoad = await ServiceIsLoadedAsync(serviceName);

        if (attemptLoad)
        {
            try
            {
                var unitPath = await _systemd.LoadUnitAsync(serviceName);
                var proxy = _bus.CreateProxy<IUnit>(DBusKnown.SystemdName, unitPath);
                await unit.LoadRuntimePropertiesAsync(proxy);
                unit.Loaded = true; // Successfully loaded
            }
            catch (DBusException)
            {
                // Handle failed load (e.g., invalid unit file)
                unit.Loaded = false;
            }
        }
        else
        {
            unit.Loaded = false;
        }

        return unit;
    }

    public async Task<List<UnixUnit>> ListServicesAsync()
    {
        var unitTups = await _systemd.ListUnitsAsync();

        var serviceNames = unitTups
            .Where(u => u.Name.EndsWith(".service"))
            .Select(u => u.Name);
        
        List<UnixUnit> services = [];

        foreach (var name in serviceNames)
        {
            var service = await GetServiceAsync(name);
            if (service is not null) services.Add(service);
        }

        return services;
    }

    public Task EnableUnitAsync(string unitName)
        => _systemd.EnableUnitFilesAsync([SystemdUtils.NormalizeServiceName(unitName)], false, true);

    public Task DisableUnitAsync(string unitName)
        => _systemd.DisableUnitFilesAsync([SystemdUtils.NormalizeServiceName(unitName)], false);

    public Task<string> GetUnitFileStateAsync(string unitName)
        => _systemd.GetUnitFileStateAsync(SystemdUtils.NormalizeServiceName(unitName));

    public void Dispose()
    {
        _bus.Dispose();
    }
}