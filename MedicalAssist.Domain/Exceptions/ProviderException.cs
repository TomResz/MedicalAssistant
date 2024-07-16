using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions;
public sealed class EmptyProviderException : BadRequestException
{
    public EmptyProviderException() : base("Provider cannot be empty or null.")
    {
        
    }
}
