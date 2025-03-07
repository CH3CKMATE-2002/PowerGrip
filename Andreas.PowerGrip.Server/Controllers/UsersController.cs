namespace Andreas.PowerGrip.Server.Controllers;

[ApiController, Route("api/[controller]")]
public class UsersController(
    ILogger<UsersController> logger,
    IAppUserManager userManager): ControllerBase
{
    private readonly ILogger<UsersController> _logger = logger;

    private readonly IAppUserManager _userManager = userManager;

    [HttpPost, Route("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] PgLoginRequest request)
    {
        _logger.LogInformation("Attempting Login for user: {username}", request.Username);

        var user = await _userManager.GetUserByUsernameAsync(request.Username);

        var invalidCreds = AuthResponse.ErrorResponse("Login Failed", ServiceError.InvalidCredentials());

        if (user is null)
        {
            _logger.LogDebug("No such user: {username}", request.Username);
            return BadRequest(invalidCreds);
        }

        if (!_userManager.CheckPassword(user, request.Password))
        {
            _logger.LogDebug("Invalid login password for user: '{username}'", request.Username);
            return BadRequest(invalidCreds);
        }

        var response = await _userManager.LoginAsync(user);

        if (response.Success)
        {
            _logger.LogDebug("Login failed for the user: {username}", request.Username);
            foreach (var error in response.Errors)
            {
                _logger.LogDebug("Login error of {username}: {error}", request.Username, error);
            }
        }

        return Ok(response);
    }

    [Authorize]
    [HttpPost, Route("logout")]
    public async Task<ActionResult<AuthResponse>> Logout([FromBody] PgLogoutRequest request)
    {
        var nameId = User.FindFirst(ClaimTypes.NameIdentifier);
        
        if (nameId is null)
        {
            return BadRequest(AuthResponse.NotRegisteredUser());
        }

        var user = await _userManager.GetUserByIdAsync(Guid.Parse(nameId.Value));

        if (user is null)
        {
            return BadRequest(AuthResponse.NotRegisteredUser());
        }

        var response = await _userManager.LogoutAsync(user, request);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [Authorize]
    [HttpPost, Route("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshRequest request)
    {
        var response = await _userManager.RefreshSessionAsync(request);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}