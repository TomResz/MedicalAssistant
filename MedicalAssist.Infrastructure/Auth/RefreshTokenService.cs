using MedicalAssist.Application.Contracts;
using MedicalAssist.Domain.ComplexTypes;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MedicalAssist.Infrastructure.Auth;
internal sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly IOptions<AuthOptions> _authOptions;

    public RefreshTokenService(IOptions<AuthOptions> authOptions)
    {
        _authOptions = authOptions;
    }

    public RefreshTokenHolder Generate(DateTime date)
    {
        var expirationTime = date.Add(_authOptions.Value.RefreshTokenExpiration ?? TimeSpan.FromHours(12));

        byte[] randomHash = new byte[64];
        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomHash);
        string hashString = Convert.ToBase64String(randomHash);

        return new(hashString, expirationTime);
    }

	public string? GetEmailFromExpiredToken(string oldAccessToken)
	{
        var claims = PrincipalsFromExpiredToken(oldAccessToken);
        var email = claims?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        return email;
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
