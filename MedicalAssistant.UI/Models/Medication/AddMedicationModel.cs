namespace MedicalAssistant.UI.Components.Medication;

public class AddMedicationModel
{
	public Guid? VisitId { get; set; }
	public string? ExtraNote { get; set; }
	public string MedicineName { get; set; }
	public int Quantity { get; set; }
	public string[] TimeOfDay { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
}
