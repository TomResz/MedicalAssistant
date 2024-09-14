using MedicalAssistant.Domain.Primitives;

namespace MedicalAssistant.Domain.Events;
public sealed record VisitDateChangedEvent(
	Guid VisitId,
	DateTime Date) : IDomainEvent;
