using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Application.Contracts;
public interface IEventPublisher
{
	Task EnqueueEventAsync(IDomainEvent @event);
}
