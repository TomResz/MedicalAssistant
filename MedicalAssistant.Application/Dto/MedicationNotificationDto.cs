namespace MedicalAssistant.Application.Dto;
public class MedicationNotificationDto
{
	public DateTime Day { get; set; }
	public Guid Id { get; set; }
	public TimeOnly Time { get; set; }
	public string MedicineName { get; set; }
	public string[] TimesOfDay { get; set; }
	public Guid MedicationId { get; set; }
}
