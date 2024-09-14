using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Application.Contracts;
public interface IVisitNotificationScheduler
{
	void Delete(string jobId);
	string ScheduleJob(VisitId visitId,VisitNotificationId visitNotificationId,DateTime scheduledTimeUtc);
}
