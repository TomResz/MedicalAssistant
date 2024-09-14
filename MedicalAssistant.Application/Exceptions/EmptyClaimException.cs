using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed  class EmptyClaimException : BadRequestException
{
    public EmptyClaimException(string claimName) : base($"Claim : {claimName} cannot be empty.")
    {
        
    }
}
