namespace Andreas.PowerGrip.Shared.Types;

public class AcAdapterInfo
{
    /// <summary>
    /// The AC adapter's identifier (e.g., AC, ACAD).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Indicates whether this AC adapter is currently supplying power.
    /// </summary>
    public bool IsOnline { get; private set; }

    /// <summary>
    /// Constructs an AcAdapterInfo instance by reading system files.
    /// </summary>
    public AcAdapterInfo(string name)
    {
        Name = name;
        UpdateData();
    }

    public void UpdateData()
    {
        string onlinePath = $"/sys/class/power_supply/{Name}/online";

        if (File.Exists(onlinePath))
            IsOnline = File.ReadAllText(onlinePath).Trim() == "1";
    }
}