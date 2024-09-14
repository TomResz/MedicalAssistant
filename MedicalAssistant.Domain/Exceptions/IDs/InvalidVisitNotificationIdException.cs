namespace MedicalAssistant.Domain.Exceptions.IDs;
public sealed class InvalidVisitNotificationIdException : BadRequestException
{
    public InvalidVisitNotificationIdException() : base("Invalid visit notification Id.")
    {
        
    }
}
