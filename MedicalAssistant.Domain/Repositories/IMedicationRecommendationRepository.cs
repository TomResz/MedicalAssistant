using MedicalAssistant.Domain.Entities;
using MedicalAssistant.Domain.ValueObjects;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Repositories;

public interface IMedicationRecommendationRepository
{
	void Delete(MedicationRecommendation medicationRecommendation);
	Task<MedicationRecommendation?> GetAsync(MedicationRecommendationId id,CancellationToken cancellationToken);
	Task<MedicationRecommendation?> GetWithNotificationsAsync(MedicationRecommendationId recommendationId, CancellationToken cancellationToken);
	Task<MedicationRecommendation?> GetByNotificationIdAsync(MedicationRecommendationNotificationId notificationId, CancellationToken cancellationToken);
	Task<bool> ExistsUsageAsync(MedicationRecommendationId recommendationId, Date date, string TimeOfDay, CancellationToken cancellationToken);
	Task<MedicationRecommendation?> GetWithUsagesAsync(MedicationRecommendationId recommendationId, CancellationToken cancellationToken);
}
