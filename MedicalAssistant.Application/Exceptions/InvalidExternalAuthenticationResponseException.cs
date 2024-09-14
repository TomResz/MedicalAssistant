using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class InvalidExternalAuthenticationResponseException : BadRequestException
{
    public InvalidExternalAuthenticationResponseException()
        : base("The external authentication response is null.")
    {
    }
}
