namespace MedicalAssistant.Application.Dto;
public class SettingsDto
{
	public bool NotificationsEnabled { get; set; }
	public bool EmailNotificationEnabled { get; set; }
	public bool VisitNotificationEnabled { get; set; }
	public string NotificationLanguage { get; set; }
}
