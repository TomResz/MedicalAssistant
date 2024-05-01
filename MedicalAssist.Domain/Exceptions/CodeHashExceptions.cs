using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;

public sealed class EmptyCodeHashException : BadRequestException
{
    public EmptyCodeHashException() : base("Empty code hash.")
    {
        
    }
}