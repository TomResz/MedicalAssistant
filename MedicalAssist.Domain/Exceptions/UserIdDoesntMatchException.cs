using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions;
public sealed class UserIdDoesntMatchException : BadRequestException
{
    public UserIdDoesntMatchException() : base($"User id doesn't match with this action.")
    {
        
    }
}
