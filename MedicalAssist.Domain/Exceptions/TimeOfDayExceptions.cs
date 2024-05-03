using MedicalAssist.Domain.Exceptions.Shared;

namespace MedicalAssist.Domain.Exceptions;

public sealed class InvalidTimeOfDayException : BadRequestException
{
    public InvalidTimeOfDayException(string value) : base($"Given time of day = '{value}' is invalid.")
    {
        
    }
}

public sealed class EmptyTimeOfDayValueException : BadRequestException
{
    public EmptyTimeOfDayValueException() : base("Time of day value cannot be null or empty.")
    {
        
    }
}