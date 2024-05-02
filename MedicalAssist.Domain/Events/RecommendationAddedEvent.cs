using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Domain.Events;
public sealed record RecommendationAddedEvent(
	Guid VisitId,
	Guid RecommendationId) : IDomainEvent;
