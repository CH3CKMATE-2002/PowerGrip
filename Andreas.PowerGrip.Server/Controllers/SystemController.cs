namespace Andreas.PowerGrip.Server.Controllers;

[Authorize]
[ApiController, Route("api/[controller]")]
public class SystemController(ILogger<SystemController> logger, IAppUserManager userManager): ControllerBase
{
    private readonly ILogger<SystemController> _logger = logger;
    
    private readonly IAppUserManager _userManager = userManager;
}