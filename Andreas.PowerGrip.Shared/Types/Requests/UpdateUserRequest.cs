namespace Andreas.PowerGrip.Shared.Types.Requests;

public class UpdateUserRequest : ModifyUserRequest
{
    public Guid Id { get; set; }
}