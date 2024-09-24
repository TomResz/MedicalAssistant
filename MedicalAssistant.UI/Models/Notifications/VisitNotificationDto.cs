using System.Text.Json.Serialization;

namespace MedicalAssistant.UI.Models.Notifications;

public class VisitNotificationDto
{
    [JsonPropertyName("scheduledDateUtc")]
    public DateTime ScheduledDateUtc { get; set; }

    [JsonPropertyName("visitId")]
    public Guid VisitId { get; set; }

    [JsonPropertyName("id")]
    public Guid Id { get; set; }

}
