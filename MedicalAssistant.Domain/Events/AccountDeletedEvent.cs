using MedicalAssistant.Domain.Primitives;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Events;

public sealed record AccountDeletedEvent(UserId UserId) : IDomainEvent;