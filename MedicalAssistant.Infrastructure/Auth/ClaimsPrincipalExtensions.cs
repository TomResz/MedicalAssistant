using MedicalAssistant.Domain.ValueObjects.IDs;
using System.Security.Authentication;
using System.Security.Claims;

namespace MedicalAssistant.Infrastructure.Auth;
internal static class ClaimsPrincipalExtensions
{
	public static UserId GetUserId(this ClaimsPrincipal? principal)
	{
		var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

		return Guid.TryParse(userId, out Guid parsedUserId)
			? parsedUserId
			: throw new AuthenticationException("User id is unavailable");
	}
}
