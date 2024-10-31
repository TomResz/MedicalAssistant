using MedicalAssistant.Domain.Entites;

namespace MedicalAssistant.Domain.Repositories;
public interface IUserSettingRepository
{
	void Update(UserSettings userSettings);
}
