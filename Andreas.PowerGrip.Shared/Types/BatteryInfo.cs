namespace Andreas.PowerGrip.Shared.Types;

public class BatteryInfo
{
    /// <summary>
    /// The battery's identifier (e.g., BAT0, BAT1).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Battery charge percentage (0-100).
    /// </summary>
    public int Percentage { get; private set; }

    /// <summary>
    /// Battery status (e.g., Charging, Discharging, Full).
    /// </summary>
    public BatteryStatus Status { get; private set; } = BatteryStatus.Unknown;

    /// <summary>
    /// Constructs a BatteryInfo instance by reading system files.
    /// </summary>
    public BatteryInfo(string name)
    {
        Name = name;
        UpdateData();
    }

    public void UpdateData()
    {
        string basePath = $"/sys/class/power_supply/{Name}";

        string capacityPath = Path.Combine(basePath, "capacity");
        string statusPath = Path.Combine(basePath, "status");

        if (File.Exists(capacityPath))
            Percentage = int.Parse(File.ReadAllText(capacityPath).Trim());

        Status = File.Exists(statusPath) ?
            Enum.Parse<BatteryStatus>(File.ReadAllText(statusPath).Trim()) :
            BatteryStatus.Unknown;
    }
}
