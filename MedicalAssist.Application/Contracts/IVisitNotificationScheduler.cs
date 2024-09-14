using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Application.Contracts;
public interface IVisitNotificationScheduler
{
	void Delete(string jobId);
	string ScheduleJob(VisitId visitId,VisitNotificationId visitNotificationId,DateTime scheduledTimeUtc);
}
