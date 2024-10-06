namespace MedicalAssistant.UI.Models.Settings;

public class SettingsViewModel
{
	public bool NotificationsEnabled { get; set; }
	public bool EmailNotificationEnabled { get; set; }
	public bool VisitNotificationEnabled { get; set; }
	public string NotificationLanguage { get; set; }
}
