using MedicalAssistant.Domain.Primitives;

namespace MedicalAssistant.Domain.Events;
public sealed record VerificationCodeRegeneratedEvent(Guid UserId, Enums.Languages Language) : IDomainEvent;
