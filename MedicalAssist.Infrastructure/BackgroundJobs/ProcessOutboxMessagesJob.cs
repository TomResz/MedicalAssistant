using MediatR;
using MedicalAssist.Domain.Abstraction;
using MedicalAssist.Domain.Primitives;
using MedicalAssist.Infrastructure.BackgroundJobs;
using MedicalAssist.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MedicalAssist.Infrastructure.Outbox;

internal sealed class ProcessOutboxMessagesJob : IProcessOutboxMessagesJob
{
	private readonly IPublisher _publisher;
	private readonly MedicalAssistDbContext _context;
	private readonly IClock _clock;
	private readonly ILogger<ProcessOutboxMessagesJob> _logger;

	private const int _size = 30;

    public ProcessOutboxMessagesJob(IPublisher publisher, MedicalAssistDbContext context, IClock clock, ILogger<ProcessOutboxMessagesJob> logger)
    {
        _publisher = publisher;
        _context = context;
        _clock = clock;
        _logger = logger;
    }

    public async Task ExecuteJob(CancellationToken cancellationToken = default)
	{
		using var transaction  = await _context.Database.BeginTransactionAsync(cancellationToken);

		List<OutboxMessage> messages = await _context
			.OutboxMessages
			.FromSql($"""
				SELECT * 
				FROM "MessageProcessing"."OutboxMessage"
				WHERE "ProcessedOnUtc" IS NULL
				LIMIT {_size}
				FOR UPDATE SKIP LOCKED
			""")
			.ToListAsync(cancellationToken);


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

				await _publisher.Publish(domainEvent, cancellationToken);
			}
			catch (Exception ex)
			{
				message.ErrorMessageJson = JsonConvert.SerializeObject(new { Message = ex.Message, StackTrace = ex.StackTrace });
                _logger.LogError($"There was an error with processing an event with id='{message.Id}' at the following time='{_clock.GetCurrentUtc()}'.");
            }
            finally
			{
				message.ProcessedOnUtc = _clock.GetCurrentUtc();
				await _context.SaveChangesAsync(cancellationToken);
			}
		}

		await transaction.CommitAsync(cancellationToken);
	}
}
