namespace Andreas.PowerGrip.Shared.Types.Responses;

public class ServiceError
{
    public ErrorKind Kind { get; set; } = ErrorKind.UnknownError;

    public string Reason { get; set; } = "Unknown Reason.";

    public override string ToString()
    {
        return $"{Kind}: {Reason}";
    }

    public static ServiceError InternalServerError(string reason) => new()
    {
        Kind = ErrorKind.InternalServerError,
        Reason = reason
    };

    public static ServiceError SessionExpired() => new()
    {
        Kind = ErrorKind.SessionExpired,
        Reason = "Session was expired; please login again"
    };
    
    public static ServiceError NotFound(string resourceTypeName) => new()
    {
        Kind = ErrorKind.NotFoundError,
        Reason = $"The requested {resourceTypeName} was not found"
    };
    
    public static ServiceError CreationFailed(string reason) => new()
    {
        Kind = ErrorKind.CreationFailed,
        Reason = reason
    };
    
    public static ServiceError UpdateFailed(string reason) => new()
    {
        Kind = ErrorKind.UpdateFailed,
        Reason = reason
    };
    
    public static ServiceError InvalidData(string reason) => new()
    {
        Kind = ErrorKind.ValidationError,
        Reason = reason
    };
    
    public static ServiceError DuplicateUsername(string username) => new()
    {
        Kind = ErrorKind.AppUsernameExists,
        Reason = $"Username '{username}' already exists"
    };
    
    public static ServiceError DuplicateEmail(string emailAddress) => new()
    {
        Kind = ErrorKind.EmailAddressInUse,
        Reason = $"Email address '{emailAddress}' is already in-use"
    };

    public static ServiceError InvalidCredentials() => new()
    {
        Kind = ErrorKind.InvalidCredentials,
        Reason = "Invalid login credentials"
    };

    public static ServiceError InvalidLoginMethod() => new()
    {
        Kind = ErrorKind.InvalidLoginMethod,
        Reason = "The login method requested is un-configured or unsupported for this request yet."
    };
}