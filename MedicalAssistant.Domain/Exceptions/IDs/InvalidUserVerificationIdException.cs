using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions.IDs;
public sealed class InvalidUserVerificationIdException : BadRequestException
{
    public InvalidUserVerificationIdException() : base("Invalid user verification id.")
    {
        
    }
}
