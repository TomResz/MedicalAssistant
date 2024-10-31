using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class UnknownMedicationRecommendationNotificationException
	: BadRequestException
{
	public UnknownMedicationRecommendationNotificationException()
		: base("Medication notification not found.")
	{

	}
}
