namespace Andreas.PowerGrip.Server.Services.Interfaces;

public interface ISystemService
{
    bool IsSystemUser(string username);

    bool Authenticate(PgLoginRequest request);

    ProcessData UpdateSystem(PgLoginRequest request);

    ProcessData UpgradeSystem(PgLoginRequest request);
}