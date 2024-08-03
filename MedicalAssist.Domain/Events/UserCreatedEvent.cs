using MedicalAssist.Domain.Enums;
using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Domain.Events;
public sealed record UserCreatedEvent(Guid UserId,Languages Language) : IDomainEvent;
