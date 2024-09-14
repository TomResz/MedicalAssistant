using MedicalAssist.Domain.ValueObjects;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Domain.Entites;
public class VisitNotification
{
	public VisitNotificationId Id { get; private set; }
	public UserId UserId { get; private set; }
	public string SimpleId { get; private set; }
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
		SimpleId = simpleId;
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
}
