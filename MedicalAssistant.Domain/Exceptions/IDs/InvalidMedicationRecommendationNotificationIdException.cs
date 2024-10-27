namespace MedicalAssistant.Domain.Exceptions.IDs;
public sealed class InvalidMedicationRecommendationNotificationIdException
	: BadRequestException
{
	public InvalidMedicationRecommendationNotificationIdException() : base("Id cannot be empty!")
	{
	}
}
