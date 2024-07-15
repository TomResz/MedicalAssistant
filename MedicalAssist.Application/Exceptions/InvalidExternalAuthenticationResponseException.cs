using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions;
public sealed class InvalidExternalAuthenticationResponseException : BadRequestException
{
    public InvalidExternalAuthenticationResponseException()
        : base("The external authentication response is null.")
    {
    }
}
