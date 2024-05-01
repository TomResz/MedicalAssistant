using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions;
public sealed class InvalidConfirmedPasswordException : BadRequestException
{
    public InvalidConfirmedPasswordException() : base("Passwords do not match.")
    {
    }
}
