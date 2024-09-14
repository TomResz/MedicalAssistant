namespace MedicalAssistant.Infrastructure.Auth;
public static class CustomClaim
{
    public const string IsVerified = "IsVerified";
    public const string IsNotVerified = "IsNotVerified";
    public const string HasExternalProvider = "HasExternalProvider";
    public const string HasInternalProvider = "HasInternalProvider";
}
