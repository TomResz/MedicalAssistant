using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.VisitNotifications.Commands.DeleteNotification;
public sealed record DeleteVisitNotificationCommand(
	Guid VisitNotificationId) : ICommand;
