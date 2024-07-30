
namespace MedicalAssist.UI.Shared.Services.User;

public interface IUserDataService
{
	Task<string?> GetUsername();
}