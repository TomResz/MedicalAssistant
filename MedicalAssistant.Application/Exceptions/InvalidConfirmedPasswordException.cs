using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class InvalidConfirmedPasswordException : BadRequestException
{
    public InvalidConfirmedPasswordException() : base("Passwords do not match.")
    {
    }
}
