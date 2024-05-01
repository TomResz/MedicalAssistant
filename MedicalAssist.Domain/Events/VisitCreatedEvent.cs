using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Domain.Events;
public sealed record VisitCreatedEvent(Guid UserId,Guid VisitId) : IDomainEvent;
