using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.MedicationRecommendations.Commands.DeleteRecommendation;
public sealed record DeleteRecommendationCommand(
    Guid Id) : ICommand;
