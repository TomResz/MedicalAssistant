using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Application.Contracts;

public interface IMedicationRecommendationNotificationScheduler
{
    void Remove(string jobId);
    void Schedule(string jobId, Date start, Date end, TimeOnly triggerTime,
        MedicationRecommendationId recommendationId, MedicationRecommendationNotificationId notificationId);
}