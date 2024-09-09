using MedicalAssist.Application.Contracts;
using MedicalAssist.Domain.Primitives;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MedicalAssist.Infrastructure.DAL.Interceptors;
internal sealed class DomainEventPublisherInterceptor : SaveChangesInterceptor
{
	private readonly ILogger<DomainEventPublisherInterceptor> _logger;
	private readonly IEventPublisher _eventPublisher;

	public DomainEventPublisherInterceptor(
		ILogger<DomainEventPublisherInterceptor> logger,
		IEventPublisher eventPublisher)
	{
		_logger = logger;
		_eventPublisher = eventPublisher;
	}

	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		var dbContext = eventData.Context;

		if (dbContext is null)
		{
			_logger.LogError($"Unknown dbContext.");
			return await base.SavingChangesAsync(eventData, result, cancellationToken);
		}
		var events = dbContext.ChangeTracker
				.Entries<IAggregateRoot>()
				.Select(x => x.Entity)
				.SelectMany(aggregate =>
				{
					IReadOnlyCollection<IDomainEvent> domainEvents = aggregate.GetEvents();

					aggregate.ClearEvents();

					return domainEvents;
				})
				.ToList();

		foreach (var domainEvent in events)
		{
			await _eventPublisher.EnqueueEventAsync(domainEvent);
		}

		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}
