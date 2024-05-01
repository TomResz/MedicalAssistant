using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions;
public sealed class EmailInUseException : BadRequestException
{
    public EmailInUseException(string email) : base($"Email: '{email}' is already in use.")
    {
    }
}
