namespace Andreas.PowerGrip.Shared.Types.Responses;

public class AuthResponse : ServiceResponse<AppJwtTokens>
{
    public bool IsTokenValid() => !string.IsNullOrWhiteSpace(Data?.AccessToken);

    public new static AuthResponse SuccessResponse(string title, AppJwtTokens? data = default) => new()
    {
        Title = title,
        Success = true,
        Data = data
    };

    public new static AuthResponse ErrorResponse(string title, IEnumerable<ServiceError> errors) => new()
    {
        Success = false,
        Title = title,
        Errors = errors.ToList(),
    };

    public new static AuthResponse ErrorResponse(string title, ServiceError error) => new()
    {
        Success = false,
        Title = title,
        Errors = [error],
    };

    public static AuthResponse FailedLoginAttempt(string title, string reason) => new()
    {
        Title = title,
        Success = false,
        Data = null,  // Unnecessary, but good as insurance.
        Errors = [
            new ServiceError { Kind = ErrorKind.AuthError, Reason = reason }
        ],
    };

    public static AuthResponse ForLoginStatus(bool succeeded, AppJwtTokens? tokens = null) => new AuthResponse
    {
        Title = succeeded ? "Login succeeded" : "Failed to login",
        Success = succeeded,
        Data = succeeded ? tokens : null,
        Errors = succeeded ? [] : [
            ServiceError.InternalServerError("Couldn't generate and save auth tokens."),
        ],
    };

    public static AuthResponse NotRegisteredUser() => new()
    {
        Title = "Not Registered",
        Success = false,
        Errors =
        [
            new ServiceError
            {
                Kind = ErrorKind.BadRequestError,
                Reason = "Endpoint failed",
            },
            new ServiceError
            {
                Kind = ErrorKind.NotFoundError,
                Reason = "User is not registered on this platform"
            },
        ],
    };
}