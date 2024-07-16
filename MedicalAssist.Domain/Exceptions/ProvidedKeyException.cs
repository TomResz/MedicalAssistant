using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions;
public sealed class EmptyProvidedKeyException : BadRequestException
{
    public EmptyProvidedKeyException() : base("Provided key cannot be empty or null.")
    {
        
    }
}
