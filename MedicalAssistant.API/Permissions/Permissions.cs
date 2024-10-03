using MedicalAssistant.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;

namespace MedicalAssistant.API.Permissions;

public static class Permissions
{
	public static AuthorizeAttribute VerifiedUser = new() { Policy = CustomClaim.IsVerified, Roles = "user" };
}
