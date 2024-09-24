namespace MedicalAssistant.UI.Models.Notifications;

public class VisitNotificationModel
{
    public Guid Id { get; set; }
    public Guid VisitId { get; set; }
    public DateTime Date { get; set; }
}
