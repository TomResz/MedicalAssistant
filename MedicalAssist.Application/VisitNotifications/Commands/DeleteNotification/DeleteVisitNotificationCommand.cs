using MediatR;

namespace MedicalAssist.Application.VisitNotifications.Commands.DeleteNotification;
public sealed record DeleteVisitNotificationCommand(
	Guid VisitNotificationId) : IRequest;
