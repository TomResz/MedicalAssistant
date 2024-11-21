namespace MedicalAssistant.UI.Models.MedicalHistory;

public class EditDiseaseStageRequest
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string? Note { get; set; }
    public Guid? VisitId { get; set; }
}