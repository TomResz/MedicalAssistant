namespace MedicalAssistant.Application.Dto;
public class RecommendationUsageDto
{
    public Guid? Id { get; set; }
    public Guid RecommendationId { get; set; }
    public string TimeOfDay { get; set; }
    public string Name { get; set; }
    public bool Status { get; set; }
    public DateTime Date { get; set; }
}
