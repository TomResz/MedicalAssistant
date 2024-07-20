using Microsoft.AspNetCore.Authorization;

namespace MedicalAssist.Infrastructure.Auth.AuthPolicy;
internal sealed class UserExternalProvider : IAuthorizationRequirement
{
    public bool HasExternalProvider { get; set; }
    public UserExternalProvider(bool hasExternalProvider)
    {
        HasExternalProvider = hasExternalProvider;
    }
}
