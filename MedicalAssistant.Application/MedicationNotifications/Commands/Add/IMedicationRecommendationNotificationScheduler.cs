using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Application.MedicationNotifications.Commands.Add;

public interface IMedicationRecommendationNotificationScheduler
{
	void Remove(string jobId);
	void Schedule(string jobId, Date start, Date end, TimeOnly triggerTime,
		MedicationRecommendationId recommendationId, MedicationRecommendationNotificationId notificationId);
}