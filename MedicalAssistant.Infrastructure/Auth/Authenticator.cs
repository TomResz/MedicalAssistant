﻿using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Security;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicalAssistant.Infrastructure.Auth;
internal sealed class Authenticator : IAuthenticator
{
    private readonly IClock _clock;
    private readonly string _issuer;
    private readonly TimeSpan _expiry;
    private readonly string _audience;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityToken = new JwtSecurityTokenHandler();

    public Authenticator(IClock clock,IOptions<AuthOptions> options)
    {
        _clock = clock;
        _issuer = options.Value.Issuer;
        _audience = options.Value.Audience;
        _expiry = options.Value.Expiry ?? TimeSpan.FromHours(1);
        _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(options.Value.SignInKey)),
                SecurityAlgorithms.HmacSha256);
    }

    public JwtDto GenerateToken(User user)
    {
        var now = _clock.GetCurrentUtc();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.Value.ToString()),
            new Claim(ClaimTypes.Name,user.FullName.Value),
            new Claim(ClaimTypes.Email,user.Email.Value),
            new Claim(ClaimTypes.Role,user.Role.Value),
            new Claim(CustomClaim.IsVerified,user.IsVerified.ToString()),
            new Claim(CustomClaim.HasExternalProvider,user.HasExternalLoginProvider.ToString()),
            new Claim(CustomClaim.IsActive,user.IsActive.ToString())
        };

        var expires = now.Add(_expiry);
        var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _signingCredentials);
        var token = _jwtSecurityToken.WriteToken(jwt);

        return new JwtDto()
        {
            AccessToken = token,
            Expiration = expires,
        };
    }
}
