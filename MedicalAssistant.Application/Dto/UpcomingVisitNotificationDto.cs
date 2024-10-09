namespace MedicalAssistant.Application.Dto;
public class UpcomingVisitNotificationDto
{
	public Guid Id { get; set; }
	public Guid VisitId { get; set; }
	public DateTime ScheduledDateUtc { get; set; }
	public DateTime Date { get; set; }
	public string DoctorName { get; set; }
	public string VisitType { get; set; }
}
