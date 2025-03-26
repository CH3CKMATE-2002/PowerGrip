namespace Andreas.PowerGrip.Server.Services.Interfaces;

public interface ISystemService
{
    bool IsSystemUser(string username);

    bool Authenticate(PgLoginRequest request);

    LaunchedProcessData UpdateSystem(PgLoginRequest request);

    LaunchedProcessData UpgradeSystem(PgLoginRequest request);
}