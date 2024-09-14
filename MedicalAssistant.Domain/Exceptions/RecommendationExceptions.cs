using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;

public class UnknownRecommendationException : BadRequestException
{
    public UnknownRecommendationException(Guid recommendationId) : base($"Recommendation with given id='{recommendationId}' was not found.")
    {
        
    }
}

public sealed class InvalidEndDateException : BadRequestException
{
    public InvalidEndDateException() : base($"Start date is greater than end date.")
    {
        
    }
}