namespace Andreas.PowerGrip.Server.Services;

public class HandshakeService(
    ILogger<HandshakeService> logger,
    AppDbContext context) : IHandshakeService
{
    public const int MaxAttempts = 10;

    private readonly TimeSpan _banTime = TimeSpan.FromMinutes(10);

    private readonly AppDbContext _context = context;

    private readonly ILogger<HandshakeService> _logger = logger;

    public ServiceResponse<HandshakeReply> Start(ConnectionInfo connection)
        => StartAsync(connection).GetAwaiter().GetResult();

    public async Task<ServiceResponse<HandshakeReply>> StartAsync(ConnectionInfo connection)
    {
        var address = connection.RemoteIpAddress;

        if (address is null) return new ServiceResponse<HandshakeReply>
        {
            Title = "Unable to Resolve Client's Address.",
            Success = false,
            Errors =
            [
                new ServiceError
                {
                    Kind = ErrorKind.NotAllowedError,
                    Reason = "You're not allowed to handshake as your IP address cannot be resolved"
                }
            ]
        };

        var handshake = await _context.Handshakes.FirstOrDefaultAsync(
            h => h.FromAddress == address);

        if (handshake is not null)
        {
            return await AttemptBanAsync(handshake);
        }
        
        return await NewHandshakeAsync(address);
    }

    public ServiceResponse Confirm(HandshakeConfirmation confirmation)
        => ConfirmAsync(confirmation).GetAwaiter().GetResult();

    public async Task<ServiceResponse> ConfirmAsync(HandshakeConfirmation confirmation)
    {
        var id = Guid.Parse(confirmation.HandshakeId);
        var handshake = await _context.Handshakes.FirstOrDefaultAsync(h => h.Id == id);

        if (handshake is null) return new ServiceResponse
        {
            Title = "Handshake Failure",
            Success = false,
            Errors =
            [
                new ServiceError
                {
                    Kind = ErrorKind.ForbiddenError,
                    Reason = "Handshake not started"
                }
            ],
        };

        try
        {
            var priv = handshake.RsaPrivateKey;
            var key = RsaUtils.Decrypt(confirmation.EncryptedKey, priv);

            handshake.AesKey = key;
            var upd = await _context.SaveChangesAsync();

            if (upd <= 0)
            {
                return new ServiceResponse
                {
                    Title = "Error Occurred During Handshake",
                    Success = false,
                    Errors =
                    [
                        ServiceError.InternalServerError("Couldn't update database"),
                    ],
                };
            }
        }
        catch
        {
            return new ServiceResponse
            {
                Success = false,
                Title = "Handshake Failure",
                Errors =
                [
                    new ServiceError
                    {
                        Kind = ErrorKind.InvalidCredentials,
                        Reason = "Invalid AES key was provided"
                    }
                ]
            };
        }

        return new ServiceResponse
        {
            Title = "Handshake Completed - AES Key Registered",
            Success = true,
        };
    }

    public bool Forget(Guid id)
        => ForgetAsync(id).GetAwaiter().GetResult();

    public async Task<bool> ForgetAsync(Guid id)
    {
        var handshake = await _context.Handshakes.FirstOrDefaultAsync(h => h.Id == id);

        if (handshake is null) return false;

        _context.Handshakes.Remove(handshake);
        var upd = await _context.SaveChangesAsync();

        return upd > 0;
    }

    #region Private Methods
    private ServiceResponse<HandshakeReply> NewHandshake(IPAddress address)
        => NewHandshakeAsync(address).GetAwaiter().GetResult();

    private async Task<ServiceResponse<HandshakeReply>> NewHandshakeAsync(IPAddress address)
    {
        var (pub, priv) = RsaUtils.GenerateKeys();

        var handshake = new HandshakeRecord
        {
            RsaPrivateKey = priv,
            RsaPublicKey = pub,
            FromAddress = address
        };

        await _context.Handshakes.AddAsync(handshake);
        var upd = await _context.SaveChangesAsync();

        if (upd <= 0) return new ServiceResponse<HandshakeReply>
        {
            Title = "Error Occurred During Handshake",
            Success = false,
            Errors =
            [
                ServiceError.InternalServerError("Couldn't update database"),
            ],
        };

        return new ServiceResponse<HandshakeReply>
        {
            Title = "Handshake Accepted",
            Success = true,
            Data = new HandshakeReply
            {
                Session = handshake.Id.ToString(),
                RsaPublicKey = pub
            },
        };
    }

    private ServiceResponse<HandshakeReply> AttemptBan(HandshakeRecord record)
        => AttemptBanAsync(record).GetAwaiter().GetResult();

    private async Task<ServiceResponse<HandshakeReply>> AttemptBanAsync(HandshakeRecord record)
    {
        if (record.Attempts >= MaxAttempts && record.IsBanned(_banTime))
        {
            return new ServiceResponse<HandshakeReply>
            {
                Success = false,
                Title = "Maximum Attempts Reached",
                Errors =
                [
                    new ServiceError
                    {
                        Kind = ErrorKind.SpamPrevention,
                        Reason = "Got maximum handshake attempts"
                    }
                ],
            };
        }

        record.Attempts++;
        var upd = await _context.SaveChangesAsync();

        if (upd <= 0) return new ServiceResponse<HandshakeReply>
        {
            Title = "Error Occurred During Handshake",
            Success = false,
            Errors =
            [
                ServiceError.InternalServerError("Couldn't update database"),
            ],
        };

        return new ServiceResponse<HandshakeReply>
        {
            Title = "Handshake Already Accepted - Awaiting Confirmation",
            Success = true,
            Data = new HandshakeReply
            {
                Session = record.Id.ToString(),
                RsaPublicKey = record.RsaPublicKey
            },
        };
    }
    #endregion
}