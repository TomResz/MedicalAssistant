using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;
public sealed class CannotAddRecommendationException : BadRequestException
{
    public CannotAddRecommendationException(string message) : base($"Cannot add recommendation to visit because : {message}")
    {
        
    }
}
