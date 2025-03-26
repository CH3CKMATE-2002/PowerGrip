namespace Andreas.PowerGrip.Shared.Linux.Enumerations;

/// <summary>
/// Contains values that PAM returns in <code>libpam.so</code>
/// </summary>
public enum PamResult: int
{
    /// <summary>Maps to PAM_SUCCESS</summary>
    PamSuccess = 0,
    
    /// <summary>Maps to PAM_OPEN_ERR</summary>
    PamOpenError = 1,
    
    /// <summary>Maps to PAM_SYMBOL_ERR</summary>
    PamSymbolError = 2,
    
    /// <summary>Maps to PAM_SERVICE_ERR</summary>
    PamServiceError = 3,
    
    /// <summary>Maps to PAM_SYSTEM_ERR</summary>
    PamSystemError = 4,
    
    /// <summary>Maps to PAM_BUF_ERR</summary>
    PamBufferError = 5,
    
    /// <summary>Maps to PAM_PERM_DENIED</summary>
    PamPermissionDenied = 6,
    
    /// <summary>Maps to PAM_AUTH_ERR</summary>
    PamAuthenticationError = 7,
    
    /// <summary>Maps to PAM_CRED_INSUFFICIENT</summary>
    PamCredInsufficient = 8,
    
    /// <summary>Maps to PAM_AUTHINFO_UNAVAIL</summary>
    PamAuthInfoUnavailable = 9,
    
    /// <summary>Maps to PAM_USER_UNKNOWN</summary>
    PamUserUnknown = 10,
    
    /// <summary>Maps to PAM_MAXTRIES</summary>
    PamMaxTries = 11,
    
    /// <summary>Maps to PAM_NEW_AUTHTOK_REQD</summary>
    PamNewAuthTokenRequired = 12,
    
    /// <summary>Maps to PAM_ACCT_EXPIRED</summary>
    PamAccountExpired = 13,
    
    /// <summary>Maps to PAM_SESSION_ERR</summary>
    PamSessionError = 14,
    
    /// <summary>Maps to PAM_AUTHTOK_EXPIRED</summary>
    PamAuthTokenExpired = 15,
    
    /// <summary>Maps to PAM_AUTHTOK_ERR</summary>
    PamAuthTokenError = 16,
    
    /// <summary>Maps to PAM_AUTHTOK_RECOVERY_ERR</summary>
    PamAuthTokenRecoveryError = 17,
    
    /// <summary>Maps to PAM_AUTHTOK_LOCK_BUSY</summary>
    PamAuthTokenLockBusy = 18,
    
    /// <summary>Maps to PAM_AUTHTOK_DISABLE_AGING</summary>
    PamAuthTokenDisableAging = 19,
    
    /// <summary>Maps to PAM_TRY_AGAIN</summary>
    PamTryAgain = 20,
    
    /// <summary>Maps to PAM_IGNORE</summary>
    PamIgnore = 21,
    
    /// <summary>Maps to PAM_ABORT</summary>
    PamAbort = 22,
    
    /// <summary>Maps to PAM_AUTHTOK_EXPIRED_ROOT</summary>
    PamRootAuthTokenExpired = 23,
    
    /// <summary>Maps to PAM_MODULE_UNKNOWN</summary>
    PamModuleUnknown = 24,
    
    /// <summary>Maps to PAM_BAD_ITEM</summary>
    PamBadItem = 25,
    
    /// <summary>Maps to PAM_CONV_ERR</summary>
    PamConversationError = 26,
    
    /// <summary>Maps to PAM_CONV_AGAIN</summary>
    PamConversationAgain = 27,
    
    /// <summary>Maps to PAM_INCOMPLETE</summary>
    PamIncomplete = 28
}