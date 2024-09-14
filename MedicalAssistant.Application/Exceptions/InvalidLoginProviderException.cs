using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class InvalidLoginProviderException : BadRequestException
{
    public InvalidLoginProviderException() : base("Incorrect login provider was used.")
    {
        
    }
}
