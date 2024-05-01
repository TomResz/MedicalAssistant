using MedicalAssist.Domain.Primitives;
using MedicalAssist.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace MedicalAssist.Infrastructure.DAL.Interceptors;
internal sealed class ConvertDomainEventToOutboxMessageInterceptor
	: SaveChangesInterceptor
{
	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		var dbContext = eventData.Context;

		if (dbContext is null)
		{
			return base.SavingChangesAsync(eventData, result, cancellationToken);
		}

		List<OutboxMessage> messages = dbContext.ChangeTracker
			.Entries<IAggregateRoot>()
			.Select(x => x.Entity)
			.SelectMany(aggregate =>
			{
				IReadOnlyCollection<IDomainEvent> domainEvents = aggregate.GetEvents();

				aggregate.ClearEvents();

				return domainEvents;
			})
			.Select(domainEvent => new OutboxMessage
			{
				Id = Guid.NewGuid(),
				OccurredOnUtc = DateTime.UtcNow,
				Type = domainEvent.GetType().Name,
				Content = JsonConvert.SerializeObject(
					domainEvent,
					new JsonSerializerSettings
					{
						TypeNameHandling = TypeNameHandling.All
					})
			})
			.ToList();

		dbContext.Set<OutboxMessage>().AddRange(messages);

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}


}
