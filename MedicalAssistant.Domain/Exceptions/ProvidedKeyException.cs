using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class EmptyProvidedKeyException : BadRequestException
{
    public EmptyProvidedKeyException() : base("Provided key cannot be empty or null.")
    {
        
    }
}
