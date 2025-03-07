namespace Andreas.PowerGrip.Server.Controllers;

[ApiController, Route("api/[controller]")]
public class DebugController(
    ILogger<DebugController> logger,
    ISystemService system,
    IJwtProvider jwtProvider) : ControllerBase
{
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ISystemService _system = system;
    private readonly ILogger<DebugController> _logger = logger;

    [HttpGet, Route("hello")]
    public ActionResult<ServiceResponse> Hello()
    {
        _logger.LogDebug("Requested '/hello' endpoint");
        return Ok(new ServiceResponse { Title = "Hello, World!" });
    }

    [HttpGet, Route("test-jwt")]
    public async Task<ActionResult<ServiceResponse<string>>> TestJwt()
    {
        _logger.LogDebug("Testing JWT functionality.");
        List<Claim> claims =
        [
            new Claim(ClaimTypes.Name, "andreas"),
        ];

        var token = await _jwtProvider.GenerateTokensAsync(claims);

        return Ok(new ServiceResponse<AppJwtTokens>
        {
            Title = "JWT Generated",
            Data = token,
        });
    }

    [HttpGet, Route("is-user/{username}")]
    public ActionResult<ServiceResponse> IsUser(string username)
    {
        _logger.LogDebug("Checking if {username} is a valid user.", username);
        var result = _system.IsSystemUser(username);

        _logger.LogDebug("Is {username} a system user? {isUser}", username, result);

        return new ServiceResponse
        {
            Title = "Checking user complete!",
            Data = result,
        };
    }

    [Authorize]
    [HttpGet, Route("whoami")]
    public ActionResult<ServiceResponse<string>> WhoAmI()
    {
        var fullNameClaim = User.FindFirst(AppClaimTypes.FullName);

        if (fullNameClaim is null)
        {
            return Ok(
                ServiceResponse<string>.ErrorResponse(
                    title: "I don't know you!",
                    error: ServiceError.NotFound("user")
                )
            );
        }

        return ServiceResponse<string>.SuccessResponse(
            title: "Hey, I know you!",
            data: $"You're {fullNameClaim.Value}"
        );
    }

    [Authorize]
    [HttpPost, Route("test-pam-auth")]
    public ActionResult<ServiceResponse<bool>> AuthenticateMe([FromBody] PassRequest request)
    {
        var sysUsernameClaim = User.FindFirst(AppClaimTypes.SystemUsername);

        if (sysUsernameClaim is null)
        {
            return Ok(
                ServiceResponse<bool>.ErrorResponse(
                    title: "Nope!",
                    error: ServiceError.NotFound("user")
                )
            );
        }

        if (request.LoginMethod != LoginMethod.PasswordLogin)
        {
            return ServiceResponse<bool>.ErrorResponse(
                title: "Authentication Error!",
                error: ServiceError.InvalidLoginMethod()
            );
        }

        var username = sysUsernameClaim.Value;
        
        var success = _system.Authenticate(new PgLoginRequest
        {
            Username = username, Password = request.Password
        });

        if (success)
        {
            return ServiceResponse<bool>.SuccessResponse(
                title: "PAM Authentication Success",
                data: success
            );
        }

        return ServiceResponse<bool>.ErrorResponse(
            title: "PAM Authentication Failed",
            error: ServiceError.InvalidCredentials()
        );
    }

    [HttpGet, Route("test-privileged")]
    public async Task<ActionResult<ServiceResponse<string>>> SystemdServiceTest()
    {
        using var client = new UdsHttpClient("/var/run/powergrip.sock");

        var response = await client.GetAsync<ServiceResponse<string>>(SystemdEndpoints.AmIRoot);

        if (response is null)
        {
            return ServiceResponse<string>.ErrorResponse(
                title: "Whoops!",
                error: ServiceError.InternalServerError("Couldn't read HTTP Content properly")
            );
        }

        return response;
    }
}