using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions.IDs;
public sealed class InvalidUserIdException : BadRequestException
{
    public InvalidUserIdException() : base($"Invalid user id.")
    {
    }
}
