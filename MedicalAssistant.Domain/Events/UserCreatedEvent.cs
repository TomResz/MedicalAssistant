using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Domain.Primitives;

namespace MedicalAssistant.Domain.Events;
public sealed record UserCreatedEvent(Guid UserId,Languages Language) : IDomainEvent;
