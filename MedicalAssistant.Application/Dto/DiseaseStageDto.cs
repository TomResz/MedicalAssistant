namespace MedicalAssistant.Application.Dto;

public class DiseaseStageDto
{
    public Guid Id { get; set; }
    public Guid MedicalHistoryId { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string? Note { get; set; }
    public VisitDto? VisitDto { get; set; }
}