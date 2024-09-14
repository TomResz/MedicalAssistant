using MediatR;

namespace MedicalAssistant.Application.VisitNotifications.Commands.DeleteNotification;
public sealed record DeleteVisitNotificationCommand(
	Guid VisitNotificationId) : IRequest;
