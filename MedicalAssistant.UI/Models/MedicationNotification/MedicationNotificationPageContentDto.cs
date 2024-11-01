namespace MedicalAssistant.UI.Models.MedicationNotification;

public class MedicationNotificationPageContentDto
{
	public Guid Id { get; set; }
	public TimeOnly Time { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public Guid MedicationId { get; set; }
	public string MedicineName { get; set; }
}
