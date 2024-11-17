namespace MedicalAssistant.UI.Models.MedicalHistory;

public class DiseaseStageViewModel
{
    public Guid? VisitId { get; set; }
    public string Name { get; set; }
    public string? Notes { get; set; }
    public DateTime? Date { get; set; }
}