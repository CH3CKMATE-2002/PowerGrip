namespace Andreas.PowerGrip.Server.Services.Interfaces;

public interface IHandshakeService
{
    public ServiceResponse<HandshakeReply> Start(ConnectionInfo connection);

    public Task<ServiceResponse<HandshakeReply>> StartAsync(ConnectionInfo connection);

    public ServiceResponse Confirm(HandshakeConfirmation confirmation);

    public Task<ServiceResponse> ConfirmAsync(HandshakeConfirmation confirmation);

    public bool Forget(Guid id);

    public Task<bool> ForgetAsync(Guid id);
}