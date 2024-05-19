using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Domain.Events;
public sealed record UserCreatedByExternalLoginProviderEvent(
    Guid UserId,string Provider) : IDomainEvent;
