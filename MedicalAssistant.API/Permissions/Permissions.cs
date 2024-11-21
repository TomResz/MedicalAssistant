using MedicalAssistant.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;

namespace MedicalAssistant.API.Permissions;

public static class Permissions
{
	public static AuthorizeAttribute IsVerifiedAndActive = new() { Policy = CustomPolicy.IsVerifiedAndActive, Roles = "user" };
	public static AuthorizeAttribute NotActiveUser = new() { Policy = CustomPolicy.NotActive, Roles = "user" };
}
