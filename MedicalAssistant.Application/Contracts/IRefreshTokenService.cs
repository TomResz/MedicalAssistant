using MedicalAssistant.Domain.ComplexTypes;
using System.Security.Claims;

namespace MedicalAssistant.Application.Contracts;
public interface IRefreshTokenService
{
    RefreshTokenHolder Generate(DateTime date);
    ClaimsPrincipal? PrincipalsFromExpiredToken(string oldAccessToken);
	string? GetEmailFromExpiredToken(string oldAccessToken);
}
