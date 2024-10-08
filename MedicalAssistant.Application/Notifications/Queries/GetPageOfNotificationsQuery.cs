using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Pagination;

namespace MedicalAssistant.Application.Notifications.Queries;
public sealed record GetPageOfNotificationsQuery(
	int Page,
	int PageSize) : IRequest<PagedList<NotificationDto>>;
