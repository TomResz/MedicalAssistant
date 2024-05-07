using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Domain.Events;
public sealed record SendEmailForPasswordChangeEvent(
    Guid UserId,string Code) : IDomainEvent;