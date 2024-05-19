using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;
public sealed class EmptyProvidedKeyException : BadRequestException
{
    public EmptyProvidedKeyException() : base("Provided key cannot be empty or null.")
    {
        
    }
}
