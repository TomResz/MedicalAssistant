using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.Repositories;

namespace MedicalAssistant.Infrastructure.DAL.Repository;
internal sealed class UserSettingsRepository : IUserSettingRepository
{
	private readonly MedicalAssistantDbContext _context;

	public UserSettingsRepository(MedicalAssistantDbContext context)
	{
		_context = context;
	}

	public void Update(UserSettings userSettings)
	{
		_context.Update(userSettings);
	}
}
