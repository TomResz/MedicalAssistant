namespace MedicalAssistant.UI.Models.Notifications;

public class NotificationModel
{
    public Guid Id { get; set; }
    public string ContentJson { get; set; }
    public bool WasRead { get; set; }
    public string Type { get; set; }
}
