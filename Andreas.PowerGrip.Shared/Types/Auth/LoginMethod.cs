namespace Andreas.PowerGrip.Shared.Types.Auth;

public enum LoginMethod: byte
{
    PasswordLogin,
    SshLogin,  // TODO: Implement it fr!
    KerberosLogin,
    // TODO: What else?
}