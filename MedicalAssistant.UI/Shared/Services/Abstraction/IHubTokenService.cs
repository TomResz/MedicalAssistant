namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IHubTokenService
{
    Task<string?> GetJwt();
}