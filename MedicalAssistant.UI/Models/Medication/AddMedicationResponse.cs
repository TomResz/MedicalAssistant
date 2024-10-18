using MedicalAssistant.UI.Models.Visits;

namespace MedicalAssistant.UI.Components.Medication;

public class AddMedicationResponse
{
	public Guid Id { get; set; }
	public VisitDto Visit { get; set; }
}
