namespace Andreas.PowerGrip.Server.Settings;

public class DatabaseSettings
{
    public DatabaseProvider Provider { get; set; }

    public string ConnectionString { get; set; } = string.Empty;

    public bool IsValid()
    {
        return Provider switch
        {
            DatabaseProvider.Sqlite => !ConnectionString.IsWhiteSpace(),
            DatabaseProvider.SqlServer => ConnectionString.StartsWith("Server="),
            _ => false,
        };
    }
}