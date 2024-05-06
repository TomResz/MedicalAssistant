using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Domain.Events;
public sealed record VisitConfirmedEvent(Guid VisitId) : IDomainEvent;
