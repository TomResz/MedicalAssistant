using Microsoft.AspNetCore.Authorization;

namespace MedicalAssistant.Infrastructure.Auth.AuthPolicy;

internal class ActiveAndVerifiedUserProvider(bool isActive, bool isVerified) : IAuthorizationRequirement
{
    public bool IsActive { get; set; } = isActive;
    public bool IsVerified { get; set; } = isVerified;
}