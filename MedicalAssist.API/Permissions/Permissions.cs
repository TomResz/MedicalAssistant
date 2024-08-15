using MedicalAssist.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;

namespace MedicalAssist.API.Permissions;

public static class Permissions
{
	public static AuthorizeAttribute VerifiedUser = new AuthorizeAttribute { Policy = CustomClaim.IsVerified, Roles = "user" };
}
