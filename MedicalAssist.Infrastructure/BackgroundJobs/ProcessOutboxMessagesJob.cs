using MediatR;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.Primitives;
using MedicalAssist.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace MedicalAssist.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob : IJob
{
	private readonly IPublisher _publisher;
	private readonly MedicalAssistDbContext _context;
	private readonly IClock _clock;
	private readonly ILogger<ProcessOutboxMessagesJob> _logger;
    public ProcessOutboxMessagesJob(IPublisher publisher, MedicalAssistDbContext context, IClock clock, ILogger<ProcessOutboxMessagesJob> logger)
    {
        _publisher = publisher;
        _context = context;
        _clock = clock;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
	{
		List<OutboxMessage> messages = await _context
			.OutboxMessages
			.Where(x => x.ProcessedOnUtc == null)
			.Take(30)
			.ToListAsync();

		foreach (OutboxMessage message in messages)
		{
			try
			{
				IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
						message.ContentJson,
						new JsonSerializerSettings
						{
							TypeNameHandling = TypeNameHandling.All
						});

				if (domainEvent is null)
				{
					_logger.LogError($"Null domain event processed on {_clock.GetCurrentUtc()}");
					continue;
				}

				await _publisher.Publish(domainEvent, context.CancellationToken);
			}
			catch (Exception ex)
			{
				message.ErrorMessageJson = JsonConvert.SerializeObject(new { Message = ex.Message, StackTrace = ex.StackTrace });
                _logger.LogError($"There was an error with processing an event with id='{message.Id}' at the following time='{_clock.GetCurrentUtc()}'.");
            }
            finally
			{
				message.ProcessedOnUtc = _clock.GetCurrentUtc();
				await _context.SaveChangesAsync(context.CancellationToken);
			}
		}
	}
}
