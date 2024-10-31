using MedicalAssistant.Domain.Entites;
using MedicalAssistant.Domain.ValueObjects.IDs;

namespace MedicalAssistant.Domain.Repositories;
public interface IMedicationRecommendationNotificationRepository
{
	Task<int> DeleteAsync(MedicationRecommendationNotificationId id,CancellationToken ct= default);
	void Update(MedicationRecommendationNotification notification);
}
