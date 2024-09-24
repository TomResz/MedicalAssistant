namespace MedicalAssistant.UI.Models.Notifications;

public class EditVisitNotificationModel
{
    public Guid Id { get; set; }
    public DateTime DateUtc { get; set; }
    public DateTime Date { get; set; }
    public DateTime CurrentDate { get; set; }
}
