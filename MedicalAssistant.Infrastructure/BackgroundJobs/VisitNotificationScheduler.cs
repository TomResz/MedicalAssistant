using Hangfire;
using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.VisitNotifications.Events.SendNotification;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Infrastructure.BackgroundJobs;
internal sealed class VisitNotificationScheduler : IVisitNotificationScheduler
{
	private readonly IBackgroundJobClient _client;
	private readonly IPublisher _publisher;
	public VisitNotificationScheduler(
		IBackgroundJobClient client,
		IPublisher publisher)
	{
		_client = client;
		_publisher = publisher;
	}

	public string ScheduleJob(VisitId visitId, VisitNotificationId visitNotificationId, DateTime scheduledTimeUtc)
	{
		DateTimeOffset offset = new (scheduledTimeUtc, TimeSpan.Zero);
		var jobId = _client.Schedule( () => Publish(visitId,visitNotificationId), offset);
		return jobId;
	}

	public async Task Publish(VisitId visitId,VisitNotificationId visitNotificationId)
		=> await _publisher.Publish(new SendVisitNotificationEvent(visitId));

	public void Delete(string jobId)
	{
		_client.Delete(jobId);
	}
}
