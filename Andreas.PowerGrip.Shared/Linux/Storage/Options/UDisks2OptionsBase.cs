namespace Andreas.PowerGrip.Shared.Linux.Storage.Options;

public abstract class UDisks2OptionsBase
{
    public bool NoUserInteraction { get; set; } = true;

    public virtual Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new()
        {
            { "auth.no_user_interaction", NoUserInteraction }
        };

        return result;
    }
}