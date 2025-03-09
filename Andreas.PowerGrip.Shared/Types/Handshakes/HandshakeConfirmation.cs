namespace Andreas.PowerGrip.Shared.Handshakes;

public class HandshakeConfirmation
{
    public string HandshakeId { get; set; } = string.Empty;

    public string EncryptedKey { get; set; } = string.Empty;
}