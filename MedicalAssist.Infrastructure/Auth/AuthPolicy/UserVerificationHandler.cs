using Microsoft.AspNetCore.Authorization;

namespace MedicalAssist.Infrastructure.Auth.AuthPolicy;
internal sealed class UserVerificationHandler : AuthorizationHandler<UserVerification>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserVerification requirement)
    {
        if (!bool.TryParse(context.User.FindFirst(c => c.Type == CustomClaim.IsVerified)?.Value, out bool isVerified))
        {
            return Task.CompletedTask;
        }

        if (isVerified == requirement.IsVerified)
        {
            context.Succeed(requirement);
        }
        return Task.CompletedTask;
    }
}
