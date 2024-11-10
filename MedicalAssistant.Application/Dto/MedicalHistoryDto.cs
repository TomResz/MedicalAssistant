namespace MedicalAssistant.Application.Dto;

public class MedicalHistoryDto
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string DiseaseName { get; set; }
    public string? Notes { get; set; }
    public string? SymptomDescription { get; set; }
    public VisitDto? VisitDto { get; set; }
    public List<DiseaseStageDto> Stages { get; set; }
}