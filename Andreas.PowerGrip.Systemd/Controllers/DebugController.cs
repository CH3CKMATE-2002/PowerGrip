namespace Andreas.PowerGrip.Systemd.Controllers;

[ApiController, Route("[controller]")]
public class DebugController : ControllerBase
{
    [HttpGet, Route("am-i-root")]
    public ActionResult<ServiceResponse<string>> IsRoot()
    {
        try
        {
            var root = UnixUser.GetCurrentUser(); // MUST BE ROOT!
            var isRoot = root.IsRoot;
            var username = root.Username;

            return Ok(new ServiceResponse<string>
            {
                Success = isRoot,
                Data = $"I am {username}"
            });
        }
        catch (Exception ex)
        {
            int err = Marshal.GetLastWin32Error();

            return BadRequest(ServiceResponse<string>.ErrorResponse(
                title: $"Checking Failed (code: {err})",
                error: ServiceError.InternalServerError(ex.Message)
            ));
        }
    }
}