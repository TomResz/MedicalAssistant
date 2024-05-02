using MediatR;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.Primitives;
using MedicalAssist.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace MedicalAssist.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob : IJob
{
	private readonly IPublisher _publisher;
	private readonly MedicalAssistDbContext _context;
	private readonly IClock _clock;
	public ProcessOutboxMessagesJob(IPublisher publisher, MedicalAssistDbContext context, IClock clock)
	{
		_publisher = publisher;
		_context = context;
		_clock = clock;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		List<OutboxMessage> messages = await _context
			.OutboxMessages
			.Where(x => x.ProcessedOnUtc == null)
			.Take(100)
			.ToListAsync();

		foreach (OutboxMessage message in messages)
		{
			try
			{
				IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
						message.Content,
						new JsonSerializerSettings
						{
							TypeNameHandling = TypeNameHandling.All
						});

				if (domainEvent is null)
				{
					continue;
				}

				await _publisher.Publish(domainEvent, context.CancellationToken);
			}
			catch (Exception ex)
			{
				message.ErrorMessage = JsonConvert.SerializeObject(new { Message = ex.Message, StackTrace = ex.StackTrace });
			}
			finally
			{
				message.ProcessedOnUtc = _clock.GetCurrentUtc();
				await _context.SaveChangesAsync(context.CancellationToken);
			}
		}
	}
}
