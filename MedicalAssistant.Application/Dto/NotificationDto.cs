namespace MedicalAssistant.Application.Dto;
public class NotificationDto
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string ContentJson { get; set; }
    public bool WasRead { get; set; }
    public DateTime PublishedDateUtc { get; set; }
}
