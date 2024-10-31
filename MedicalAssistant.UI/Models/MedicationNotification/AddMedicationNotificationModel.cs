namespace MedicalAssistant.UI.Models.MedicationNotification;

public class AddMedicationNotificationModel
{
    public Guid MedicationRecommendationId { get; set; }
    public DateTime Start { get; set; }
	public DateTime End { get; set; }
    public TimeOnly TriggerTimeUtc { get; set; }

}

//Guid MedicationRecommendationId,
//DateTime Start,
//	DateTime End,
//	TimeOnly TriggerTimeUtc