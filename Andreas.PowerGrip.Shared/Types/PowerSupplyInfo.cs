namespace Andreas.PowerGrip.Shared.Types;

public class PowerSupplyInfo
{
    /// <summary>
    /// List of detected battery sources.
    /// </summary>
    public List<BatteryInfo> Batteries { get; } = [];

    /// <summary>
    /// List of detected AC power sources.
    /// </summary>
    public List<AcAdapterInfo> AcAdapters { get; } = [];

    /// <summary>
    /// Indicates whether the system has at least one battery.
    /// </summary>
    public bool HasBattery => Batteries.Count > 0;

    /// <summary>
    /// Indicates whether the system is plugged into AC power.
    /// </summary>
    public bool IsPluggedIn => AcAdapters.Any(ac => ac.IsOnline);

    /// <summary>
    /// Detects and loads all power supply sources dynamically.
    /// </summary>
    public PowerSupplyInfo()
    {
        var powerSupplies = Directory.GetDirectories("/sys/class/power_supply/")
            .Select(Path.GetFileName)
            .ToArray();

        foreach (var supply in powerSupplies)
        {
            string typePath = $"/sys/class/power_supply/{supply}/type";

            if (!File.Exists(typePath))
                continue; // Ignore if type file doesn't exist

            string type = File.ReadAllText(typePath).Trim();
            if (type.Equals("Battery", StringComparison.OrdinalIgnoreCase))
            {
                Batteries.Add(new BatteryInfo(supply!));
            }
            else if (type.Equals("Mains", StringComparison.OrdinalIgnoreCase) ||
                     type.Equals("USB", StringComparison.OrdinalIgnoreCase))
            {
                AcAdapters.Add(new AcAdapterInfo(supply!));
            }
        }
    }
    
    public void Update()
    {
        Batteries.ForEach(bat => bat.UpdateData());
        AcAdapters.ForEach(ac => ac.UpdateData());
    }
}
