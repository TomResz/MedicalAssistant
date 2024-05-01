using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions;
public sealed class InvalidSignInCredentialsException : BadRequestException
{
    public InvalidSignInCredentialsException() : base("Invalid email or password.")
    {
    }
}
