using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Application.Exceptions;
public sealed class InvalidSignInCredentialsException : BadRequestException
{
    public InvalidSignInCredentialsException() : base("Invalid email or password.")
    {
    }
}
