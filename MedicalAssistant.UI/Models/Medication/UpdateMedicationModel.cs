using MedicalAssistant.UI.Components.Medication;

namespace MedicalAssistant.UI.Models.Medication;

public class UpdateMedicationModel : AddMedicationModel
{
    public Guid Id { get; set; }
}