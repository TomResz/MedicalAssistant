namespace MedicalAssistant.UI.Models.Notifications;

public class VisitNotificationWithDetailsModel
{
	public Guid Id { get; set; }
	public DateTime ScheduledDateUtc { get; set; }
	public Guid VisitId { get; set; }
	public DateTime Date { get; set; }
	public string DoctorName { get; set; }
	public string VisitType { get; set; }
}
