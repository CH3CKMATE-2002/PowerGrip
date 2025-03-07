namespace Andreas.PowerGrip.Shared.Types.Requests;

public class DeleteUserRequest
{
    public Guid Id { get; set; }

    public string Reason { get; set; } = string.Empty;
}