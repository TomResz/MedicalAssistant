using MediatR;

namespace MedicalAssistant.Application.UserSettings.Commands.Update;
public sealed record UpdateUserSettingsCommand(
		bool NotificationsEnabled,
		bool EmailNotificationEnabled,
		bool VisitNotificationEnabled,
		string NotificationLanguage) : IRequest;