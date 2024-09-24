using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.VisitNotifications.Commands.AddNotifications;
public sealed record AddVisitNotificationCommand(
	Guid VisitId,
	DateTime ScheduledDate,
	DateTime ScheduledDateUtc,
	DateTime CurrentDate) : IRequest<VisitNotificationDto>;
