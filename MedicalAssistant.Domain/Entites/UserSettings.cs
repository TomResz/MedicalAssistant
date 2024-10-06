using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entites;
public class UserSettings
{
	public UserId UserId { get; private set; }
	public User User { get; private set; }
	public NotificationLanguage NotificationLanguage { get; private set; }
	public bool NotificationsEnabled { get; private set; }
	public bool EmailNotificationEnabled { get; private set; }
	public bool VisitNotificationEnabled { get; private set; }

    private UserSettings(
		UserId userId,
		NotificationLanguage notificationLanguage,
		bool notificationsEnabled,
		bool emailNotificationEnabled,
		bool visitNotificationEnabled)
    {
		UserId = userId;
		NotificationLanguage = notificationLanguage;
		NotificationsEnabled = notificationsEnabled;
		EmailNotificationEnabled = emailNotificationEnabled;
		VisitNotificationEnabled = visitNotificationEnabled;
    }

	public static UserSettings Create(
		UserId userId,
		NotificationLanguage notificationLanguage)
	{
		return new UserSettings(
			userId,
			notificationLanguage,
			true,
			  true,
			  true);
	}

	public void ChangeSetting(
		bool notificationsEnabled,
		bool emailNotificationEnabled,
		bool visitNotificationEnabled,
		NotificationLanguage notificationLanguage)
	{
		NotificationsEnabled = notificationsEnabled;
		EmailNotificationEnabled = emailNotificationEnabled;
		VisitNotificationEnabled = visitNotificationEnabled;
		NotificationLanguage = NotificationLanguage;
	}
}
