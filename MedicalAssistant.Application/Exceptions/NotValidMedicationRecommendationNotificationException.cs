using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class NotValidMedicationRecommendationNotificationException
	: BadRequestException
{
    public NotValidMedicationRecommendationNotificationException()
        : base("Notification is not valid.")
    {
        
    }
}
