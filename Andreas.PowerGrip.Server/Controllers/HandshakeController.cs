namespace Andreas.PowerGrip.Server.Controllers;

// TODO: Implement this!
// 4. Think of a way to move the anonymous user into
//    an authenticated user.
//    (Moving the key from anon to known user)
//    Maybe use the "Session ID" we create
//    until the user creates an account? In which the
//    changes are going to be saved?
//! No authorization here!

[ApiController, Route("api/[controller]")]
public class HandshakeController(
    ILogger<HandshakeController> logger,
    AppDbContext context,
    IHandshakeService handshake) : ControllerBase
{
    private readonly ILogger<HandshakeController> _logger = logger;
    private readonly AppDbContext _context = context;
    private readonly IHandshakeService _handshake = handshake;

    [HttpGet, Route("start")]
    public async Task<ActionResult<ServiceResponse<HandshakeReply>>> StartHandshake()
    {
        var connection = HttpContext.Connection;
        var response = await _handshake.StartAsync(connection);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost, Route("confirm")]
    public async Task<ActionResult<ServiceResponse>> ConfirmHandshake([FromBody] HandshakeConfirmation confirmation)
    {
        var response = await _handshake.ConfirmAsync(confirmation);
        return response.Success ? Ok(response) : BadRequest(response);
    }
}