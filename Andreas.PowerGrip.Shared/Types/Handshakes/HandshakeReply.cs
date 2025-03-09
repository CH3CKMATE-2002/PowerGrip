namespace Andreas.PowerGrip.Shared.Handshakes;

public class HandshakeReply
{
    public string Session { get; set; } = string.Empty;

    public string RsaPublicKey { get; set; } = string.Empty;
}