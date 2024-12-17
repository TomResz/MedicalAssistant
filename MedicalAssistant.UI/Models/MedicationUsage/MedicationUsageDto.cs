namespace MedicalAssistant.UI.Models.MedicationUsage;

public class MedicationUsageDto
{
	public Guid? Id { get; set; }
	public Guid RecommendationId { get; set; }
	public string TimeOfDay { get; set; }
    public DateTime Date { get; set; }
	public bool Status { get; set; }
	public string Name { get; set; }

}
