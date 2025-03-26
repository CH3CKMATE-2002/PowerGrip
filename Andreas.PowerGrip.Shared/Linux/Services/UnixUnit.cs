using systemd1.DBus;

namespace Andreas.Test.LinuxProcesses.Utilities.Services;

/// <summary>
/// Represents detailed information about a systemd unit.
/// </summary>
public class UnixUnit : IDisposable
{
    private static readonly TimeSpan _timeout = TimeSpan.FromSeconds(2);

    private SemaphoreSlim _operationLock = new(1, 1);

    private IUnit? _proxy = null;

    internal async Task LoadRuntimePropertiesAsync(IUnit? proxy = null)
    {
        if (proxy is not null) _proxy = proxy;
        if (_proxy is null) return;

        var props = await _proxy.GetAllAsync();

        Name = props.Names.FirstOrDefault(string.Empty);
        Description = props.Description;
        ActiveState = EnumUtils.ParseEnumMemberValue<UnitActiveState>(props.ActiveState, UnitActiveState.Unknown);
        LoadState = EnumUtils.ParseEnumMemberValue<UnitLoadState>(props.LoadState, UnitLoadState.Unknown);
        JobId = props.Job.Item1;
        JobPath = props.Job.Item2.ToString();
        Path = props.SourcePath;
        Following = props.Following;
        SubState = props.SubState;

        Dependencies = props.Requires;
        Dependents = props.RequiredBy;
        After = props.After;
        Before = props.Before;
        Conflicts = props.Conflicts;
        Wants = props.Wants;
        WantedBy = props.WantedBy;
    }

    public Task RefreshAsync() => LoadRuntimePropertiesAsync(_proxy);

    /// <summary>
    /// The name of the unit.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// A brief description of the unit.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// The active state of the unit.
    /// </summary>
    public UnitActiveState ActiveState { get; set; } = UnitActiveState.Unknown;

    /// <summary>
    /// The load state of the unit.
    /// </summary>
    public UnitLoadState LoadState { get; set; } = UnitLoadState.Unknown;

    /// <summary>
    /// The sub-state of the unit, providing additional details about its status.
    /// </summary>
    public string SubState { get; set; } = string.Empty;

    public string[] Dependencies { get; set; } = [];

    public string[] Dependents { get; set; } = [];

    public string[] After { get; set; } = [];

    public string[] Before { get; set; } = [];

    public string[] Conflicts { get; set; } = [];

    public string[] Wants { get; set; } = [];

    public string[] WantedBy { get; set; } = [];

    /// <summary>
    /// If the unit follows another unit, this field contains the name of the followed unit.
    /// </summary>
    public string Following { get; set; } = string.Empty;

    /// <summary>
    /// The file path of the unit.
    /// </summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>
    /// The ID of any active job associated with the unit.
    /// </summary>
    public uint JobId { get; set; }

    /// <summary>
    /// The type of job currently associated with the unit.
    /// </summary>
    public string JobType { get; set; } = string.Empty;

    /// <summary>
    /// The path of the job associated with the unit.
    /// </summary>
    public string JobPath { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the service is loaded into systemd.
    /// </summary>
    public bool Loaded { get; set; }

    /// <summary>
    /// The state of the unit file associated with the service.
    /// </summary>
    /// <value><see cref="UnitFileState.Unknown"/></value>
    public UnitFileState FileState { get; set; } = UnitFileState.Unknown;

    public bool IsEnabled => FileState == UnitFileState.Enabled;

    public StatusSummary Summary
    {
        get
        {
            return ActiveState switch
            {
                UnitActiveState.Active => StatusSummary.Running,
                UnitActiveState.Failed => StatusSummary.Stopped,
                UnitActiveState.Inactive when FileState == UnitFileState.Disabled
                    => StatusSummary.Disabled,
                UnitActiveState.Inactive => StatusSummary.Stopped,
                _ => StatusSummary.Unknown  // Appears when the service is not loaded.
            };
        }
    }

    public async Task StartAsync()
    {
        await _operationLock.WaitAsync();
        try
        {
            if (_proxy is null)
                throw new InvalidOperationException("Service not loaded");

            await _proxy.StartAsync("replace");
            await WaitForStateAsync(UnitActiveState.Active);
        }
        finally
        {
            _operationLock.Release();
        }
    }

    public async Task StopAsync()
    {
        await _operationLock.WaitAsync();
        try
        {
            if (_proxy is null)
                throw new InvalidOperationException("Service not loaded");

            await _proxy.StopAsync("replace");
            await WaitForStateAsync(UnitActiveState.Inactive);
        }
        finally
        {
            _operationLock.Release();
        }
    }

    public async Task RestartAsync()
    {
        await _operationLock.WaitAsync();
        try
        {
            if (_proxy is null)
                throw new InvalidOperationException("Service not loaded");

            await _proxy.RestartAsync("replace");
            await WaitForStateAsync(UnitActiveState.Active);
        }
        finally
        {
            _operationLock.Release();
        }
    }

    public async Task ResetFailedAsync()
    {
        await _operationLock.WaitAsync();
        try
        {
            if (_proxy is null)
                throw new InvalidOperationException("Service not loaded");

            await _proxy.ResetFailedAsync();
            await WaitForStateAsync(UnitActiveState.Active);
        }
        finally
        {
            _operationLock.Release();
        }
    }

    private async Task WaitForStateAsync(UnitActiveState targetState)
    {
        var sw = Stopwatch.StartNew();

        while (sw.ElapsedMilliseconds < _timeout.TotalMilliseconds)
        {
            await RefreshAsync();
            if (ActiveState == targetState) break;
            await Task.Delay(100);
        }

        sw.Stop();
    }

    public void Dispose()
    {
        _operationLock.Dispose();
    }
}