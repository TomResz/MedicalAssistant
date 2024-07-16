using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Application.Exceptions;
public sealed class EmailInUseException : ConflictException
{
    public EmailInUseException(string email) : base($"Email: '{email}' is already in use.")
    {
    }
}
