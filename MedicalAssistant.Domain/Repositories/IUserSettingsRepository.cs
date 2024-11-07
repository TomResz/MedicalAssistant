using MedicalAssistant.Domain.Entities;

namespace MedicalAssistant.Domain.Repositories;
public interface IUserSettingRepository
{
	void Update(UserSettings userSettings);
}
