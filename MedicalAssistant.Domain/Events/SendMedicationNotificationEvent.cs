using MedicalAssistant.Domain.Primitives;

namespace MedicalAssistant.Domain.Events;

public sealed record SendMedicationNotificationEvent(
	Guid RecommendationId,
	Guid NotificationId) : IDomainEvent;