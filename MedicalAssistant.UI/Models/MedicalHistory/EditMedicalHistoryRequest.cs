using MedicalAssistant.UI.Components.MedicalHistory;

namespace MedicalAssistant.UI.Models.MedicalHistory;

public class EditMedicalHistoryRequest : EditMedicalHistoryViewModel
{
    public Guid Id { get; set; }
}