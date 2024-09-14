using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class InvalidSignInCredentialsException : BadRequestException
{
    public InvalidSignInCredentialsException() : base("Invalid email or password.")
    {
    }
}
