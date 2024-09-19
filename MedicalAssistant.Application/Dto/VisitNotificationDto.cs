namespace MedicalAssistant.Application.Dto;
public class VisitNotificationDto
{
    public Guid Id { get; set; }
    public Guid VisitId { get; set; }
    public DateTime ScheduledDateUtc { get; set; }
}
