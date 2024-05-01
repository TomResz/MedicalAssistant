namespace MedicalAssist.Domain.Primitives;
public abstract class AggregateRoot<TId> : IAggregateRoot
{
	public TId Id { get; protected set; }
	private readonly List<IDomainEvent> _events = new();

	public IReadOnlyCollection<IDomainEvent> GetEvents() => new List<IDomainEvent>(_events);
	protected void AddEvent(IDomainEvent @event) => _events.Add(@event);

	public void ClearEvents() => _events.Clear();
}