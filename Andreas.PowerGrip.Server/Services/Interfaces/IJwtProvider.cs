namespace Andreas.PowerGrip.Server.Services.Interfaces;

public interface IJwtProvider
{
    AppJwtTokens GenerateTokens(IEnumerable<Claim> claims);

    Task<AppJwtTokens> GenerateTokensAsync(IEnumerable<Claim> claims);
}