using MedicalAssistant.Domain.Primitives;

namespace MedicalAssistant.Domain.Events;
public sealed record RecommendationAddedEvent(
	Guid VisitId,
	Guid RecommendationId) : IDomainEvent;
