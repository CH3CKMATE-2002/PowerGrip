namespace Andreas.PowerGrip.Shared.Types.Requests;

public class PgLoginRequest: PassRequest
{
    public string Username { get; set; } = string.Empty;
}