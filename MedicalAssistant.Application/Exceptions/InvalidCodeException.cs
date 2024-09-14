using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;
public sealed class InvalidCodeException : BadRequestException
{
    public InvalidCodeException(string code) : base($"Given code='{code}' is not correct.")
    {
        
    }
}
