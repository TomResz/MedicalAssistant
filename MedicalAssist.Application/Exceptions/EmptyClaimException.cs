using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Application.Exceptions;
public sealed  class EmptyClaimException : BadRequestException
{
    public EmptyClaimException(string claimName) : base($"Claim : {claimName} cannot be empty.")
    {
        
    }
}
