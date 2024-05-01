using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions.IDs;
public sealed class InvalidRecommendationIdException : BadRequestException
{
    public InvalidRecommendationIdException() : base("Invalid recommendation Id.")
    {
    }
}
