using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class UserNotFoundException : BadRequestException
{
    public UserNotFoundException() : base("User with given credentials not found.")
    {
        
    }
    public UserNotFoundException(string email) : base($"User with given email='{email}' not found.")
    {

    }
}
