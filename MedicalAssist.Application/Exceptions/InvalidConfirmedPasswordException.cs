using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Application.Exceptions;
public sealed class InvalidConfirmedPasswordException : BadRequestException
{
    public InvalidConfirmedPasswordException() : base("Passwords do not match.")
    {
    }
}
