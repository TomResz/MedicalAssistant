﻿using MedicalAssistant.Domain.Primitives;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Events;

public sealed record AccountDeactivatedEvent(UserId UserId) : IDomainEvent;