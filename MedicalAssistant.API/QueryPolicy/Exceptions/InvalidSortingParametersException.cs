using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.API.QueryPolicy.Exceptions;

public sealed class InvalidSortingParametersException : BadRequestException
{
    public InvalidSortingParametersException() : base("Invalid sorting parameter.")
    {
        
    }
}
