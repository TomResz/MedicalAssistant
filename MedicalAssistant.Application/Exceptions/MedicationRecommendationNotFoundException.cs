using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class MedicationRecommendationNotFoundException
	: BadRequestException
{
    public MedicationRecommendationNotFoundException(Guid id) : base($"Recommendation with Id='{id}' was not found.")
    {
        
    }
}
