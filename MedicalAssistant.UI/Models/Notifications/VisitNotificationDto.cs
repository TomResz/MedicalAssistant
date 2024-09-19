namespace MedicalAssistant.UI.Models.Notifications;

public class VisitNotificationDto
{
    public DateTime ScheduledDateUtc { get; set; }
    public Guid VisitId { get; set; }
    public Guid Id { get; set; }

}
