using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.API.QueryPolicy.Exceptions;

public sealed class InvalidSortingParametersException : BadRequestException
{
    public InvalidSortingParametersException() : base("Invalid sorting parameter.")
    {
        
    }
}
