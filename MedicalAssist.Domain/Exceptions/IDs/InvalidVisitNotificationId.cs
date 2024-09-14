namespace MedicalAssist.Domain.Exceptions.IDs;
public sealed class InvalidVisitNotificationId : BadRequestException
{
    public InvalidVisitNotificationId() : base("Invalid visit notification Id.")
    {
        
    }
}
