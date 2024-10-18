using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Commands.AddRecommendation;

public class AddMedicationRecommendationResponse
{
    public Guid Id { get; set; }
    public VisitDto? Visit { get; set; }
}
