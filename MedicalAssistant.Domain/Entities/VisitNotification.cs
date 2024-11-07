using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Entities;
public class VisitNotification
{
	public VisitNotificationId Id { get; private set; }
	public UserId UserId { get; private set; }
	public string JobId { get; private set; }
	public Date ScheduledDateUtc { get; private set; }
	public VisitId VisitId { get; private set; }

	private VisitNotification() { }
    private VisitNotification(
		VisitNotificationId id,
		UserId userId,
		string simpleId,
		Date scheduledDateUtc,
		VisitId visitId)
    {
        Id = id;
		UserId = userId;
		JobId = simpleId;
		ScheduledDateUtc = scheduledDateUtc;
		VisitId = visitId;
    }
	
	public static VisitNotification Create(
		VisitNotificationId id,
		UserId userId,
		string simpleId,
		Date scheduledDateUtc,
		VisitId visitId)
	{
		VisitNotification notification = new(
			id,
			userId,
			simpleId,
			scheduledDateUtc,
			visitId);
		return notification;
	}

	internal void ChangeDate(Date dateUtc)
	{
		ScheduledDateUtc = dateUtc;
	}

	public void ChangeJobId(string jobId)
	{
		JobId = jobId;
	}
}
