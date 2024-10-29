using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationNotifications.Queries;
public sealed record GetMedicationNotificationByWeekQuery(
	int Offset,
	DateTime Date) : IRequest<IEnumerable<MedicationNotificationDto>>;
