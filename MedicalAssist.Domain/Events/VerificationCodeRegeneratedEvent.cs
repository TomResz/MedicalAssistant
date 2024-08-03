using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Domain.Events;
public sealed record VerificationCodeRegeneratedEvent(Guid UserId, Enums.Languages Language) : IDomainEvent;
