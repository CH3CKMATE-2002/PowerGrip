namespace Andreas.PowerGrip.Shared.Linux.Storage.Options;

public class LuksOptions : UDisks2OptionsBase
{
    public string PassPhrase { get; set; } = string.Empty;
    public LuksKeyDerivation KeyDerivation { get; set; } = LuksKeyDerivation.None;
    public EncryptionScheme EncryptionType { get; set; } = EncryptionScheme.Unknown;
    public ulong Iterations { get; set; } = 0;
    public ulong MemoryCostKb { get; set; } = 0;
    public ulong TimeCostMs { get; set; } = 0;

    private int _numberOfThreads = 1;
    public int NumberOfThreads
    {
        get { return _numberOfThreads; }
        set { _numberOfThreads = Math.Clamp(value, 1, 4); }
    }

    public override Dictionary<string, object> ToDictionary()
    {
        var result = base.ToDictionary();

        if (!string.IsNullOrEmpty(PassPhrase))
        {
            result.Add("encrypt.passphrase", PassPhrase);
            if (EncryptionType != EncryptionScheme.Unknown)
                result.Add("encrypt.type", EncryptionType.ToString().ToLower());
            if (KeyDerivation != LuksKeyDerivation.None)
                result.Add("encrypt.pbkdf", KeyDerivation.ToString().ToLower());
            if (Iterations != 0) result.Add("encrypt.iterations", Iterations);
            if (MemoryCostKb != 0) result.Add("encrypt.memory", MemoryCostKb);
            if (TimeCostMs != 0) result.Add("encrypt.time", TimeCostMs);
            result.Add("encrypt.threads", NumberOfThreads);
        }

        return result;
    }
}