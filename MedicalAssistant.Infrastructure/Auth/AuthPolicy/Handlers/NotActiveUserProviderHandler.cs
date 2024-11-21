using Microsoft.AspNetCore.Authorization;

namespace MedicalAssistant.Infrastructure.Auth.AuthPolicy.Handlers;

internal sealed class NotActiveUserProviderHandler : AuthorizationHandler<NotActiveUserProvider>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, NotActiveUserProvider requirement)
    {
        if (!bool.TryParse(context.User.FindFirst(c => c.Type == CustomClaim.IsActive)?.Value, out bool isActive))
        {
            return Task.CompletedTask;
        }

        if (isActive == requirement.IsActive)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}