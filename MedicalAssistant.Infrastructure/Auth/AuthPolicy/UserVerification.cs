using Microsoft.AspNetCore.Authorization;

namespace MedicalAssistant.Infrastructure.Auth.AuthPolicy;
internal sealed class UserVerification : IAuthorizationRequirement
{
    public UserVerification(bool isVerified)
    {
        IsVerified = isVerified;
    }

    public bool IsVerified { get; set; }
}
