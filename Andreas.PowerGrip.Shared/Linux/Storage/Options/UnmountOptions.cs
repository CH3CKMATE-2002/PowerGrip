namespace Andreas.PowerGrip.Shared.Linux.Storage.Options;

public class UnmountOptions : UDisks2OptionsBase
{
    public bool Force { get; set; } = false;

    public override Dictionary<string, object> ToDictionary()
    {
        var result = base.ToDictionary();

        result.Add("force", Force);

        return result;
    }
}