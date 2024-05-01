using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Domain.Events;
public sealed record UserCreatedEvent(Guid UserId) : IDomainEvent;
