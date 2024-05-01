using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions.IDs;
public sealed class InvalidUserVerificationIdException : BadRequestException
{
    public InvalidUserVerificationIdException() : base("Invalid user verification id.")
    {
        
    }
}
