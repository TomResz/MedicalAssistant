namespace MedicalAssistant.API.Models.DiseaseStages;

public class EditDiseaseStageModel
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public string? Note { get; set; }
    public Guid? VisitId { get; set; }
}