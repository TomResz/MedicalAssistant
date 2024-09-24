namespace MedicalAssistant.Domain.Exceptions;
public sealed class InvalidVisitNotificationDateException : BadRequestException
{
    public InvalidVisitNotificationDateException() : base("Date cannot be greater or equal than visit date.")
    {
        
    }
}
