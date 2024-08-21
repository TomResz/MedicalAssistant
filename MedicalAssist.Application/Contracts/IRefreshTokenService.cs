using MedicalAssist.Domain.ComplexTypes;
using System.Security.Claims;

namespace MedicalAssist.Application.Contracts;
public interface IRefreshTokenService
{
    RefreshTokenHolder Generate(DateTime date);
    ClaimsPrincipal? PrincipalsFromExpiredToken(string oldAccessToken);
	string? GetEmailFromExpiredToken(string oldAccessToken);
}
