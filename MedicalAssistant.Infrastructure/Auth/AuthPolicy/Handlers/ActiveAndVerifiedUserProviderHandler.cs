using Microsoft.AspNetCore.Authorization;

namespace MedicalAssistant.Infrastructure.Auth.AuthPolicy.Handlers;

internal sealed class ActiveAndVerifiedUserProviderHandler : AuthorizationHandler<ActiveAndVerifiedUserProvider>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveAndVerifiedUserProvider requirement)
    {
        if (!bool.TryParse(context.User.FindFirst(c => c.Type == CustomClaim.IsActive)?.Value, out bool isActive) ||
            !bool.TryParse(context.User.FindFirst(c => c.Type == CustomClaim.IsVerified)?.Value, out bool isVerified))
        {
            return Task.CompletedTask;
        }

        if (isActive == requirement.IsActive &&
            isVerified == requirement.IsVerified)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}