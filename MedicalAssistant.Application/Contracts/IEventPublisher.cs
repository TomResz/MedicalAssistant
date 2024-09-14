using MedicalAssistant.Domain.Primitives;

namespace MedicalAssistant.Application.Contracts;
public interface IEventPublisher
{
	Task EnqueueEventAsync(IDomainEvent @event);
}
