namespace Andreas.PowerGrip.Shared.Types.Requests;

public class CreateUserRequest : ModifyUserRequest
{
    public IEnumerable<string> Roles { get; set; } = [];
}