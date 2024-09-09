using Hangfire;
using MediatR;
using MedicalAssist.Application.Contracts;
using MedicalAssist.Domain.Primitives;

namespace MedicalAssist.Infrastructure.BackgroundJobs;
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
