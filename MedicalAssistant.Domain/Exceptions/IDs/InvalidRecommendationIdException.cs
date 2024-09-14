using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions.IDs;
public sealed class InvalidRecommendationIdException : BadRequestException
{
    public InvalidRecommendationIdException() : base("Invalid recommendation Id.")
    {
    }
}
