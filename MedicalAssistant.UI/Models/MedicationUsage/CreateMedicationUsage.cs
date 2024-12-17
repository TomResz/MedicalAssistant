namespace MedicalAssistant.UI.Models.MedicationUsage;

public class CreateMedicationUsage
{
    public Guid RecommendationId { get; set; }
    public DateTime Date { get; set; }
    public string TimeOfDay { get; set; }
}