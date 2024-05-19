using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions;
public sealed  class EmptyClaimException : BadRequestException
{
    public EmptyClaimException(string claimName) : base($"Claim : {claimName} cannot be empty.")
    {
        
    }
}
