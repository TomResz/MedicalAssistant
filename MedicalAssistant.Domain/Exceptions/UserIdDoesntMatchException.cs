using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class UserIdDoesntMatchException : BadRequestException
{
    public UserIdDoesntMatchException() : base($"User id doesn't match with this action.")
    {
        
    }
}
