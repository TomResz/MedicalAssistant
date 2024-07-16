using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions.IDs;
public sealed class InvalidRecommendationIdException : BadRequestException
{
    public InvalidRecommendationIdException() : base("Invalid recommendation Id.")
    {
    }
}
