using MedicalAssistant.Domain.ComplexTypes;
using MedicalAssistant.Domain.ValueObjects.IDs;
using System.Security.Claims;

namespace MedicalAssistant.Application.Contracts;
public interface IRefreshTokenService
{
    TokenHolder Generate(DateTime date,UserId userId);
    ClaimsPrincipal? PrincipalsFromExpiredToken(string oldAccessToken);
	UserId? GetUserIdFromExpiredToken(string oldAccessToken);
}
