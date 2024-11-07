using MedicalAssistant.Domain.Entities;

namespace MedicalAssistant.Domain.Policies;
public interface IMedicationRecommendationPolicy
{
	bool ValidNotificationProperties(MedicationRecommendation recommendation,MedicationRecommendationNotification notification);
}
