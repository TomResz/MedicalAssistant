using Microsoft.AspNetCore.Authorization;

namespace MedicalAssistant.Infrastructure.Auth.AuthPolicy;

internal class NotActiveUserProvider(bool isActive) : IAuthorizationRequirement
{
    public bool IsActive { get; set; } = isActive;
}