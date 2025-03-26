namespace Andreas.PowerGrip.Systemd.Services;

public class OnStartupService(
    ILogger<OnStartupService> logger,
    IHostApplicationLifetime lifetime,
    UdsOptions udsOptions) : IHostedService
{
    private readonly ILogger<OnStartupService> _logger = logger;
    private readonly IHostApplicationLifetime _lifetime = lifetime;
    private readonly UdsOptions _udsOptions = udsOptions;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var socketPath = _udsOptions.SocketFile;

        _lifetime.ApplicationStarted.Register(() =>
        {
            try
            {
                // Set permissions (e.g., 660 for group access)
                UnixFileMode perms = UnixFileMode.UserRead | UnixFileMode.UserWrite
                                    | UnixFileMode.GroupRead | UnixFileMode.GroupWrite;
                bool permsFixed = PosixUtils.ModifyPermissions(socketPath, perms);

                // Change group ownership (if server has permissions)
                bool ownFixed = PosixUtils.ChangeOwnership(
                    socketPath,
                    UnixUser.GetUser("root").UserId,
                    UnixGroup.GetGroup("powergrip").GroupId
                );

                _logger.LogInformation("Socket permissions set: {PermsFixed}", permsFixed);
                _logger.LogInformation("Socket ownership set: {OwnFixed}", ownFixed);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to adjust socket permissions/ownership.");
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        var socketPath = _udsOptions.SocketFile;

        if (File.Exists(socketPath))
        {
            File.Delete(socketPath);
        }
        
        return Task.CompletedTask;
    }
}