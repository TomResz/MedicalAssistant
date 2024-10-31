using MedicalAssistant.Domain.Entites;

namespace MedicalAssistant.Domain.Policies;
public interface IMedicationRecommendationPolicy
{
	bool ValidNotificationProperties(MedicationRecommendation recommendation,MedicationRecommendationNotification notification);
}
