﻿namespace MedicalAssist.Domain.Primitives;
public interface IAggregateRoot
{
	IReadOnlyCollection<IDomainEvent> GetEvents();
	void ClearEvents();
}
