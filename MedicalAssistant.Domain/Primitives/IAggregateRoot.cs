namespace MedicalAssistant.Domain.Primitives;
public interface IAggregateRoot
{
	IReadOnlyCollection<IDomainEvent> GetEvents();
	void ClearEvents();
}
