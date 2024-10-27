using MedicalAssistant.UI.Models.Visits;

namespace MedicalAssistant.UI.Models.Medication;

public class MedicationDto
{
    public Guid Id { get; set; }
    public VisitDto? Visit { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Name { get; set; }
    public string[] TimeOfDay { get; set; }
    public int Quantity { get; set; }
    public string? ExtraNote { get; set; }

}