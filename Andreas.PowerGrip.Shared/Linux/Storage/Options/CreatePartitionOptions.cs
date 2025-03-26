namespace Andreas.PowerGrip.Shared.Linux.Storage.Options;


public class CreatePartitionOptions : UDisks2OptionsBase
{
    public Guid? UUID { get; set; } = null;

    public DosPartitionType PartitionType { get; set; } = DosPartitionType.Unknown;

    public override Dictionary<string, object> ToDictionary()
    {
        var result = base.ToDictionary();

        if (UUID is not null) result.Add("partition-uuid", UUID.Value.ToString());
        if (PartitionType is not DosPartitionType.Unknown) result.Add("partition-type", PartitionType.ToString());
        
        return result;
    }
}