using MediatR;

namespace MedicalAssistant.Application.MedicationRecommendations.Commands.AddUsage;
public sealed record AddRecommendationUsageCommand(
	Guid RecommendationId,
	DateTime Date,
	string TimeOfDay) : IRequest;