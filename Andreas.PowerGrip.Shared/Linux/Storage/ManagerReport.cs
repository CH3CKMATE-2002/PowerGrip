namespace Andreas.PowerGrip.Shared.Linux.Storage;

public class ManagerReport
{
    public bool HasAbility { get; set; } = false;

    public string Required { get; set; } = string.Empty;

    public StorageResizeFlags ResizeFlags { get; set; } = StorageResizeFlags.None;
}