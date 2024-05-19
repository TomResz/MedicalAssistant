using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions;
public sealed class InvalidLoginProviderException : BadRequestException
{
    public InvalidLoginProviderException() : base("Incorrect login provider was used.")
    {
        
    }
}
