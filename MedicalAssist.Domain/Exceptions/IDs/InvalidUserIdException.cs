using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions.IDs;
public sealed class InvalidUserIdException : BadRequestException
{
    public InvalidUserIdException() : base($"Invalid user id.")
    {
    }
}
