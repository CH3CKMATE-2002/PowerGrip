namespace Andreas.PowerGrip.Shared.Linux;

public class AcAdapterInfo
{
    /// <summary>
    /// The AC adapter's identifier (e.g., AC, ACAD).
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Indicates whether this AC adapter is currently supplying power.
    /// </summary>
    public bool IsOnline { get; private set; }

    private AcAdapterInfo() { }

    /// <summary>
    /// Constructs an AcAdapterInfo instance by reading system files.
    /// </summary>
    public static AcAdapterInfo GetInfo(string name)
    {
        var info = new AcAdapterInfo();
        info.Name = name;
        info.UpdateData();
        return info;
    }

    public void UpdateData()
    {
        string onlinePath = $"/sys/class/power_supply/{Name}/online";

        if (File.Exists(onlinePath))
            IsOnline = File.ReadAllText(onlinePath).Trim() == "1";
    }
}