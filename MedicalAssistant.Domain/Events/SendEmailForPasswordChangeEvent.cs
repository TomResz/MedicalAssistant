using MedicalAssistant.Domain.Primitives;

namespace MedicalAssistant.Domain.Events;
public sealed record SendEmailForPasswordChangeEvent(
    Guid UserId,string Code, Enums.Languages Language) : IDomainEvent;