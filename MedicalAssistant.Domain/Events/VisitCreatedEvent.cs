using MedicalAssistant.Domain.Primitives;

namespace MedicalAssistant.Domain.Events;
public sealed record VisitCreatedEvent(Guid UserId,Guid VisitId) : IDomainEvent;
