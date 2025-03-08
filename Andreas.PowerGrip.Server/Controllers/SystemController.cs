namespace Andreas.PowerGrip.Server.Controllers;

[Authorize]
[ApiController, Route("api/[controller]")]
public class SystemController(ILogger<SystemController> logger, IAppUserManager userManager): ControllerBase
{
    private readonly ILogger<SystemController> _logger = logger;
    
    private readonly IAppUserManager _userManager = userManager;

    [HttpGet, Route("update")]
    public async Task<ActionResult<ServiceResponse<ProcessData>>> SendSystemUpdateRequest()
    {
        // TODO: Make this better (Remove the string parameter)
        using var client = new UdsHttpClient("/var/run/powergrip.sock");
        var result = await client.GetAsync<ServiceResponse<ProcessData>>(SystemdEndpoints.AptUpdate);
        
        return result is not null && result.Success ? Ok(result) : BadRequest(result);
    }
}