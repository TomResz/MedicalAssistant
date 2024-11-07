namespace MedicalAssistant.Application.Dto.Mappers;
public static class SettingsMapper
{
	public static SettingsDto ToDto(this Domain.Entities.UserSettings settings)
		=> new()
		{
			EmailNotificationEnabled = settings.EmailNotificationEnabled,
			NotificationLanguage = settings.NotificationLanguage.ToString(),
			VisitNotificationEnabled = settings.VisitNotificationEnabled,
			NotificationsEnabled = settings.NotificationsEnabled
		};
}
