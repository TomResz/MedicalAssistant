using MediatR;

namespace MedicalAssistant.Application.MedicationRecommendations.Commands.AddRecommendation;
public sealed record AddMedicationRecommendationCommand(
    Guid? VisitId,
    string? ExtraNote,
    string MedicineName,
    int Quantity,
    string[] TimeOfDay,
    DateTime StartDate,
    DateTime EndDate) : IRequest<AddMedicationRecommendationResponse>;
