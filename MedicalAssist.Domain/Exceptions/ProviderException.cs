using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;
public sealed class EmptyProviderException : BadRequestException
{
    public EmptyProviderException() : base("Provider cannot be empty or null.")
    {
        
    }
}
