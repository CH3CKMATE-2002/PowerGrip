namespace Andreas.PowerGrip.Shared.Types.Responses;


/// <summary>
/// An enum that represents various error types from PowerGrip.
/// </summary>
public enum ErrorKind : byte
{
    /// <summary>
    /// Represents an unknown error.
    /// </summary>
    UnknownError,

    /// <summary>
    /// Represents an authorization/authentication error.
    /// </summary>
    AuthError,

    /// <summary>
    /// Represents a data validation error.
    /// </summary>
    ValidationError,

    /// <summary>
    /// Represents an error related to resource not found.
    /// </summary>
    NotFoundError,

    /// <summary>
    /// Represents an error related to a conflict in the request, such as duplicate data.
    /// </summary>
    ConflictError,

    /// <summary>
    /// Represents an error due to a bad request.
    /// </summary>
    BadRequestError,

    /// <summary>
    /// Represents an error due to an operation not allowed.
    /// </summary>
    NotAllowedError,

    /// <summary>
    /// Represents an error due to exceeding a limit, such as rate limiting.
    /// </summary>
    LimitExceededError,

    /// <summary>
    /// Represents an error due to a service being unavailable.
    /// </summary>
    ServiceUnavailableError,

    /// <summary>
    /// Represents an error due to an internal server error.
    /// </summary>
    InternalServerError,

    /// <summary>
    /// Represents an error related to endpoint security issues.
    /// </summary>
    EndpointSecurityError,

    /// <summary>
    /// Represents an error related to web application security issues.
    /// </summary>
    WebApplicationSecurityError,

    /// <summary>
    /// Represents an error related to data backup and recovery issues.
    /// </summary>
    DataBackupRecoveryError,

    /// <summary>
    /// Represents an error related to encryption/decryption tool issues.
    /// </summary>
    EncryptionError,

    /// <summary>
    /// Represents an error related to system update issues.
    /// </summary>
    SystemUpdateError,

    /// <summary>
    /// Represents an error related to file permission issues.
    /// </summary>
    FilePermissionError,

    /// <summary>
    /// Represents an error related to unusual login attempt detection.
    /// </summary>
    UnusualLoginAttemptError,

    /// <summary>
    /// Represents an error related to geo-location analysis issues.
    /// </summary>
    GeoLocationAnalysisError,

    /// <summary>
    /// Represents an error related to monitoring 404 errors on a web service.
    /// </summary>
    NotFoundMonitoringError,

    /// <summary>
    /// Represents an error where creation of an object has failed.
    /// </summary>
    CreationFailed,

    /// <summary>
    /// Represents an error when the user fails a login.
    /// </summary>
    InvalidCredentials,

    /// <summary>
    /// Represents an error when the username already exists.
    /// </summary>
    AppUsernameExists,

    /// <summary>
    /// Represents an error when the email address already in-use.
    /// </summary>
    EmailAddressInUse,

    /// <summary>
    /// Represents an error when the update of an object has failed.
    /// </summary>
    UpdateFailed,

    /// <summary>
    /// Represents an error when the current user session has been expired.
    /// </summary>
    SessionExpired,

    /// <summary>
    /// Represents an error when the user tries to login using an un-configured or unsupported type. 
    /// </summary>
    InvalidLoginMethod,
}