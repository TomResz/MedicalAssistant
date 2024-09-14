using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions.IDs;
public sealed class InvalidUserIdException : BadRequestException
{
    public InvalidUserIdException() : base($"Invalid user id.")
    {
    }
}
