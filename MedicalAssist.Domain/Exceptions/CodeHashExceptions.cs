using MedicalAssist.Domain.Exceptions;

namespace MedicalAssist.Domain.Exceptions;

public sealed class EmptyCodeHashException : BadRequestException
{
    public EmptyCodeHashException() : base("Empty code hash.")
    {
        
    }
}