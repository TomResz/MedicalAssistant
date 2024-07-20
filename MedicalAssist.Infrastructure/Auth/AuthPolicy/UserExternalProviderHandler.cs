using Microsoft.AspNetCore.Authorization;

namespace MedicalAssist.Infrastructure.Auth.AuthPolicy;
internal sealed class UserExternalProviderHandler : AuthorizationHandler<UserExternalProvider>
{
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserExternalProvider requirement)
	{
		if (!bool.TryParse(context.User.FindFirst(c => c.Type == CustomClaim.HasExternalProvider)?.Value, out bool hasExternalProvider))
		{
			return Task.CompletedTask;
		}

		if (hasExternalProvider == requirement.HasExternalProvider)
		{
			context.Succeed(requirement);
		}
		return Task.CompletedTask;
	}
}
