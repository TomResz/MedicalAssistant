using Hangfire;
using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Primitives;

namespace MedicalAssistant.Infrastructure.BackgroundJobs;
internal sealed class EventPublisher : IEventPublisher
{
	private readonly IPublisher _publisher;
	private readonly IBackgroundJobClient _backgroundJobClient;

	public EventPublisher(
		IMediator publisher,
		IBackgroundJobClient backgroundJobClient)
	{
		_publisher = publisher;
		_backgroundJobClient = backgroundJobClient;
	}

	public Task EnqueueEventAsync(IDomainEvent @event)
	{
		_backgroundJobClient.Enqueue(() => PublishEvent(@event));
		return Task.CompletedTask;
	}

	public async Task PublishEvent(IDomainEvent domainEvent)
	{
		await _publisher.Publish(domainEvent);
	}
}
