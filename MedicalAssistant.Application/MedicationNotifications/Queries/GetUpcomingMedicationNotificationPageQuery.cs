using MediatR;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Pagination;

namespace MedicalAssistant.Application.MedicationNotifications.Queries;

public sealed record GetUpcomingMedicationNotificationPageQuery(
	int Page,
	int PageSize,
	int Offset,
	DateTime Date) : IRequest<PagedList<MedicationNotificationPageContentDto>>;