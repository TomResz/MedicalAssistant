using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Domain.Events;
public sealed record SendEmailForPasswordChangeEvent(
    Guid UserId,string Code, Enums.Languages Language) : IDomainEvent;