using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationNotifications.Queries;
public sealed record GetMedicationNotificationByWeekQuery(
	double Offset,
	DateTime Date) : IQuery<IEnumerable<MedicationNotificationDto>>;
