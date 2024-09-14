using MedicalAssistant.Domain.Primitives;

namespace MedicalAssistant.Domain.Events;
public sealed record RecommendationDeletedEvent(
	Guid VisitId,Guid RecommendationId) : IDomainEvent;