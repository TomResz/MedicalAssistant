using MediatR;

namespace MedicalAssistant.Application.MedicationRecommendations.Commands.DeleteRecommendation;
public sealed record DeleteRecommendationCommand(
    Guid Id) : IRequest;
