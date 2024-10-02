using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MedicalAssistant.Infrastructure.Auth;
internal sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly IOptions<AuthOptions> _authOptions;

    public RefreshTokenService(IOptions<AuthOptions> authOptions)
    {
        _authOptions = authOptions;
    }

    public TokenHolder Generate(DateTime date,UserId userId)
    {
        var expirationTime = date.Add(_authOptions.Value.RefreshTokenExpiration ?? TimeSpan.FromHours(12));

        byte[] randomHash = new byte[64];
        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomHash);
        string hashString = Convert.ToBase64String(randomHash);

        return TokenHolder.Create(hashString, expirationTime, userId);
    }

	public UserId? GetUserIdFromExpiredToken(string oldAccessToken)
	{
        var claims = PrincipalsFromExpiredToken(oldAccessToken);
        var idValue = claims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        if (Guid.TryParse(idValue, out var id))
        {
            return id;
        }
        
        return null;
	}

	public ClaimsPrincipal? PrincipalsFromExpiredToken(string oldAccessToken)
    {
        var tokenParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.Value.SignInKey)),
            ValidIssuer = _authOptions.Value.Issuer,
            ValidAudience = _authOptions.Value.Audience,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
        };

        var handler = new JwtSecurityTokenHandler();

        return handler.ValidateToken(oldAccessToken, tokenParameters, out _);
    }
}
