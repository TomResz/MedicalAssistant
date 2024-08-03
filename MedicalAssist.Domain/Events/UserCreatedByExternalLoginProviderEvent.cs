using MedicalAssist.Domain.Enums;
using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Domain.Events;
public sealed record UserCreatedByExternalLoginProviderEvent(
    Guid UserId,string Provider,Languages Language) : IDomainEvent;
