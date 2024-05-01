using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions;
public sealed class UnverifiedUserException : BadRequestException
{
    public UnverifiedUserException() : base("Unverified user.")
    {
    }
}
