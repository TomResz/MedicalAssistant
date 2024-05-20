using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;
public sealed class UserIdDoesntMatchException : BadRequestException
{
    public UserIdDoesntMatchException() : base($"User id doesn't match with this action.")
    {
        
    }
}
