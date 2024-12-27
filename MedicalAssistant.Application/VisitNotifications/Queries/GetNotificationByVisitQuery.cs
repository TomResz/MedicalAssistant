using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.VisitNotifications.Queries;
public sealed record GetNotificationByVisitQuery(
	Guid VisitId) : IQuery<IEnumerable<VisitNotificationDto>>;
