namespace Andreas.PowerGrip.Shared.Types;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DatabaseProvider
{
    Sqlite,
    SqlServer,
}