namespace Andreas.PowerGrip.Systemd.Controllers;

[ApiController, Route("[controller]")]
public class SystemController: ControllerBase
{
    [HttpGet, Route("i-am-root")]
    public ActionResult<ServiceResponse<string>> IsRoot()
    {
        try
        {
            var isRoot = UnixUtils.IsRoot();
            var username = UnixUtils.GetUsername();

            return Ok(new ServiceResponse<string>
            {
                Success = isRoot,
                Data = $"I am {username}"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ServiceResponse<string>.ErrorResponse(
                title: "Checking Failed",
                error: ServiceError.InternalServerError(ex.Message)
            ));
        }
    }
}