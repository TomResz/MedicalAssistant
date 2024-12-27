using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.UserSettings.Commands.Update;
public sealed record UpdateUserSettingsCommand(
		bool NotificationsEnabled,
		bool EmailNotificationEnabled,
		bool VisitNotificationEnabled,
		string NotificationLanguage) : ICommand;