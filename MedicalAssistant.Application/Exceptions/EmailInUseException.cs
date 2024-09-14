using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class EmailInUseException : ConflictException
{
    public EmailInUseException(string email) : base($"Email: '{email}' is already in use.")
    {
    }
}
