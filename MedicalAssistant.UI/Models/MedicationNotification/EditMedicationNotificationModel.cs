namespace MedicalAssistant.UI.Models.MedicationNotification;

public class EditMedicationNotificationModel
{
    public Guid Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public TimeOnly TriggerTimeUtc { get; set; }
}
