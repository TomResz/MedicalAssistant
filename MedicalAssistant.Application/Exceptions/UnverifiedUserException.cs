using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class UnverifiedUserException : BadRequestException
{
    public UnverifiedUserException() : base("Unverified user.")
    {
    }
}
