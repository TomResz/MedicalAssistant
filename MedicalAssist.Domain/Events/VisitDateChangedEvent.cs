using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Domain.Events;
public sealed record VisitDateChangedEvent(
	Guid VisitId,
	DateTime Date) : IDomainEvent;
