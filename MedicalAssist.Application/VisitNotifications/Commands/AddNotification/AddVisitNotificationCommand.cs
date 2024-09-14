using MediatR;
using MedicalAssist.Application.Dto;

namespace MedicalAssist.Application.VisitNotifications.Commands.AddNotifications;
public sealed record AddVisitNotificationCommand(
	Guid VisitId,DateTime ScheduledDateUtc) : IRequest<VisitNotificationDto>;
