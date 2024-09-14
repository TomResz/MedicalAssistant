using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;
public sealed class EmptyProviderException : BadRequestException
{
    public EmptyProviderException() : base("Provider cannot be empty or null.")
    {
        
    }
}
