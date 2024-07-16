using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Application.Exceptions;
public sealed class InvalidLoginProviderException : BadRequestException
{
    public InvalidLoginProviderException() : base("Incorrect login provider was used.")
    {
        
    }
}
