using MedicalAssistant.Domain.Entites;

namespace MedicalAssistant.Domain.Policies;
internal sealed class MedicationRecommendationPolicy
	: IMedicationRecommendationPolicy
{
	public bool ValidNotificationProperties(MedicationRecommendation recommendation, MedicationRecommendationNotification notification)
	{
		if (recommendation.StartDate.ToDate() > notification.Start.ToDate() ||
			recommendation.EndDate.ToDate() < recommendation.EndDate.ToDate())
		{
			return false;
		}
		return true;
	}
}
