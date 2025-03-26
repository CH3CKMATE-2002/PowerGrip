namespace Andreas.PowerGrip.Shared.Linux.Storage.Options;

public class BlockFormatOptions : LuksOptions
{
    public string Label { get; set; } = string.Empty;
    public Guid? UUID { get; set; } = null;
    public bool TakeOwnership { get; set; } = false;
    public EraseScheme EraseScheme { get; set; } = EraseScheme.None;

    public override Dictionary<string, object> ToDictionary()
    {
        var result = base.ToDictionary();

        if (!string.IsNullOrEmpty(Label)) result.Add("label", Label);
        if (UUID != null) result.Add("uuid", UUID.Value.ToString("D")); // Format as "00000000-0000-0000-0000-000000000000"
        if (TakeOwnership) result.Add("take-ownership", TakeOwnership);
        if (EraseScheme != EraseScheme.None)
            result.Add("erase", EraseScheme.GetEnumMemberValue()); // e.g., "ata-secure-erase"

        return result;
    }
}