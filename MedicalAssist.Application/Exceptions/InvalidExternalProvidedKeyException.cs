using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions;
public sealed class InvalidExternalProvidedKeyException : BadRequestException
{
    public InvalidExternalProvidedKeyException() : base("Provided key doesn't match.")
    {
        
    }
}
