namespace Andreas.PowerGrip.Server.Services;

public class JwtProvider : IJwtProvider
{
    private readonly ILogger<JwtProvider> _logger;

    private readonly JwtSettings _settings;

    public JwtProvider(ILogger<JwtProvider> logger, JwtSettings settings)
    {
        _logger = logger;
        _settings = settings;
        _logger.LogDebug("JWT provider is ready for use.");
    }

    public AppJwtTokens GenerateTokens(IEnumerable<Claim> claims)
    {
        _logger.LogDebug("Generating JWT with claims: {claims}", string.Join(", ", claims.Select(c => $"({c.Type}, {c.Value})")));

        var key = _settings.ResolveSigningKey();
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var creationDate = DateTime.UtcNow;

        var securityToken = new JwtSecurityToken(
            issuer: _settings.ValidIssuer,
            audience: _settings.ValidAudience,
            claims: claims,
            expires: creationDate.Add(_settings.Lifetime),
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        var refreshToken = RandomUtils.RandomBase64Bytes(64);

        return new AppJwtTokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessLifetime = _settings.Lifetime,
            RefreshLifetime = _settings.RefreshLifetime,
            CreatedAt = creationDate,
        };
    }

    public async Task<AppJwtTokens> GenerateTokensAsync(IEnumerable<Claim> claims)
        => await Task.Run(() => GenerateTokens(claims));
}