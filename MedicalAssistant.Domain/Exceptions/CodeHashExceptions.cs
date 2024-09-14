using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Domain.Exceptions;

public sealed class EmptyCodeHashException : BadRequestException
{
    public EmptyCodeHashException() : base("Empty code hash.")
    {
        
    }
}