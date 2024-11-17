namespace MedicalAssistant.UI.Models.MedicalHistory;

public class AddDiseaseStageRequest
{
    public Guid? VisitId { get; set; }
    public string Name { get; set; }
    public string? Note { get; set; }
    public DateTime Date { get; set; }
}