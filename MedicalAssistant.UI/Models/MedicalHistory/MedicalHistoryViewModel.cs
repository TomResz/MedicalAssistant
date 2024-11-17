namespace MedicalAssistant.UI.Models.MedicalHistory;

public class MedicalHistoryViewModel
{
    public string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public string? Notes { get; set; }
    public string? SymptomDescription { get; set; }
    public Guid? VisitId { get; set; }
}