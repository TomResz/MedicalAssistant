using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Domain.Events;
public sealed record RecommendationDeletedEvent(
	Guid VisitId,Guid RecommendationId) : IDomainEvent;