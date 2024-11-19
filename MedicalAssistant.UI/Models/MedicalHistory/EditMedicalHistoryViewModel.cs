namespace MedicalAssistant.UI.Models.MedicalHistory;

public class EditMedicalHistoryViewModel
{
    public string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? SymptomDescription { get; set; }
    public string? Notes { get; set; }
    public Guid? VisitId { get; set; }
}