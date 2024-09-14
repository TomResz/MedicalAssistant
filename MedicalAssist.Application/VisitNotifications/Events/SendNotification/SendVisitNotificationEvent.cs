using MediatR;

namespace MedicalAssist.Application.VisitNotifications.Events.SendNotification;
public sealed record SendVisitNotificationEvent(
	Guid VisitId) : INotification;
