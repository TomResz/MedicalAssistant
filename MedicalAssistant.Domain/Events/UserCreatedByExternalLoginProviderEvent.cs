using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Domain.Primitives;

namespace MedicalAssistant.Domain.Events;
public sealed record UserCreatedByExternalLoginProviderEvent(
    Guid UserId,string Provider,Languages Language) : IDomainEvent;
