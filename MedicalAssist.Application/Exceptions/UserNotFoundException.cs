using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions;
public sealed class UserNotFoundException : BadRequestException
{
    public UserNotFoundException() : base("User with given credentials not found.")
    {
        
    }
}
