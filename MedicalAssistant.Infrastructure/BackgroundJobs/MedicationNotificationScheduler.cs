using Hangfire;
using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Domain.Abstraction;
using MedicalAssistant.Domain.Events;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;
using Microsoft.Extensions.Logging;

namespace MedicalAssistant.Infrastructure.BackgroundJobs;
internal class MedicationNotificationScheduler : IMedicationRecommendationNotificationScheduler
{
	private readonly IRecurringJobManager _recurringJobManager;
	private readonly ILogger<MedicationNotificationScheduler> _logger;
	private readonly IPublisher _publisher;
	private readonly IClock _clock;

	public MedicationNotificationScheduler(
		IRecurringJobManager recurringJobManager,
		IPublisher publisher,
		IClock clock,
		ILogger<MedicationNotificationScheduler> logger)
	{
		_recurringJobManager = recurringJobManager;
		_publisher = publisher;
		_clock = clock;
		_logger = logger;
	}

	public void Schedule(string jobId, Date start, Date end, TimeOnly triggerTime, MedicationRecommendationId recommendationId, MedicationRecommendationNotificationId notificationId)
	{
		string cronExpression = $"{triggerTime.Minute} {triggerTime.Hour} * * *";

		_recurringJobManager.AddOrUpdate(
			jobId,
			() => Publish(jobId, (DateTime)start, (DateTime)end, recommendationId, notificationId),
			cronExpression);
	}

	public async Task Publish(string jobId, DateTime start, DateTime end, Guid recommendationId, Guid notificationId)
	{
		var currentUtc = _clock.GetCurrentUtc();

		var onlyDate = currentUtc.Date;

		if (start.Date > onlyDate)
		{
			return;
		}

		if (end.Date < onlyDate)
		{
			Remove(jobId);
			_logger.LogInformation($"Expired job with Id={jobId} has been deleted.");
			return;
		}

		_logger.LogInformation($"Job with Id={jobId}, executed at {currentUtc.ToString("HH:mm dd-MM-yyyy")} UTC.");

		await _publisher.Publish(new SendMedicationNotificationEvent(recommendationId, notificationId));

		if (end.Date == onlyDate)
		{
			Remove(jobId);
			_logger.LogInformation($"Job with Id={jobId} has been deleted.");
			return;
		}

	}

	public void Remove(string jobId)
	{
		_recurringJobManager.RemoveIfExists(jobId);
	}
}
