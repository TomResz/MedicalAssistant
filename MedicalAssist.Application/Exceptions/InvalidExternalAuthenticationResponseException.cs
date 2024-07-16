using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Application.Exceptions;
public sealed class InvalidExternalAuthenticationResponseException : BadRequestException
{
    public InvalidExternalAuthenticationResponseException()
        : base("The external authentication response is null.")
    {
    }
}
