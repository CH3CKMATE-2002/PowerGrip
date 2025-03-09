namespace Andreas.PowerGrip.Server.Controllers;

[Authorize]
[ApiController, Route("api/[controller]")]
public class SystemController(ILogger<SystemController> logger, IAppUserManager userManager, UdsHttpClient client): ControllerBase
{
    private readonly ILogger<SystemController> _logger = logger;
    
    private readonly IAppUserManager _userManager = userManager;

    private readonly UdsHttpClient _client = client;

    [HttpGet, Route("update")]
    public async Task<ActionResult<ServiceResponse<ProcessData>>> SendSystemUpdateRequest()
    {
        var result = await _client.GetAsync<ServiceResponse<ProcessData>>(SystemdEndpoints.AptUpdate);
        return result is not null && result.Success ? Ok(result) : BadRequest(result);
    }
}