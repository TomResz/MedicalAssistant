using MedicalAssistant.Domain.Exceptions;

namespace MedicalAssistant.Application.Exceptions;

public sealed class ScheduledDateCannotBeGreatestThanDateException : BadRequestException
{
    public ScheduledDateCannotBeGreatestThanDateException() : base("Scheduled date cannot be greater than visit date.")
    {
        
    }
}
