namespace Andreas.PowerGrip.Shared.Types.Requests;

public class PassRequest
{
    public LoginMethod LoginMethod { get; set; } = LoginMethod.PasswordLogin;  // the default is using a password!

    public string Password { get; set; } = string.Empty;

    public EncryptionType Encryption { get; set; } = EncryptionType.None;
}