using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationNotifications.Queries;
public sealed record GetMedicationNotificationByMedicationQuery(
	Guid MedicationId,
	double Offset) : IQuery<IEnumerable<MedicationNotificationWithDateRangeDto>>;
