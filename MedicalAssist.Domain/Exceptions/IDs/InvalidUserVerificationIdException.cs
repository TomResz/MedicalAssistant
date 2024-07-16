using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions.IDs;
public sealed class InvalidUserVerificationIdException : BadRequestException
{
    public InvalidUserVerificationIdException() : base("Invalid user verification id.")
    {
        
    }
}
