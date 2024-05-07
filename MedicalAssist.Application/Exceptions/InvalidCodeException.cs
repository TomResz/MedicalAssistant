using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Application.Exceptions;
public sealed class InvalidCodeException : BadRequestException
{
    public InvalidCodeException(string code) : base($"Given code='{code}' is not correct.")
    {
        
    }
}
