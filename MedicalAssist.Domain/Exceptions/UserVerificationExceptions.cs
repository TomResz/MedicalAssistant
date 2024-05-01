using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;

public sealed class SameCodeHashesException : BadRequestException
{
    public SameCodeHashesException() : base("The new code must be different from the old one.")
    {
        
    }
}

public sealed class InvalidNewExpirationDate : BadRequestException
{
    public InvalidNewExpirationDate() : base("New expiration date must be greater than the old one.")
    {
        
    }
}