namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IUserDataService
{
    Task<string?> GetUsername();
}