using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Repositories;

public interface IMedicationRecommendationRepository
{
	void Delete(MedicationRecommendation medicationRecommendation);
	Task<MedicationRecommendation?> GetAsync(MedicationRecommendationId id,CancellationToken cancellationToken);
	Task<MedicationRecommendation?> GetWithNotificationsAsync(MedicationRecommendationId recommendationId, CancellationToken cancellationToken);
	Task<MedicationRecommendation?> GetByNotificationIdAsync(MedicationRecommendationNotificationId notificationId, CancellationToken cancellationToken);
}
