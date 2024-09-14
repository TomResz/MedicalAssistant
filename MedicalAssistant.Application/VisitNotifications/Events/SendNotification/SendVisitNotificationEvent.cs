using MediatR;

namespace MedicalAssistant.Application.VisitNotifications.Events.SendNotification;
public sealed record SendVisitNotificationEvent(
	Guid VisitId) : INotification;
