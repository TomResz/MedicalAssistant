using MediatR;

namespace MedicalAssistant.Application.MedicationNotifications.Commands.Add;
public sealed record AddMedicationNotificationCommand(
	Guid MedicationRecommendationId,
	DateTime Start,
	DateTime End,
	TimeOnly TriggerTimeUtc) : IRequest;
