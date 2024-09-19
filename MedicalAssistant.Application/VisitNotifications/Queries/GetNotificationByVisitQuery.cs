using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.VisitNotifications.Queries;
public sealed record GetNotificationByVisitQuery(
	Guid VisitId) : IRequest<IEnumerable<VisitNotificationDto>>;
