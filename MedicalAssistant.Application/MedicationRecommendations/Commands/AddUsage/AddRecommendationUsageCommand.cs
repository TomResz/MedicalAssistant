using MedicalAssistant.Application.Abstraction;

namespace MedicalAssistant.Application.MedicationRecommendations.Commands.AddUsage;
public sealed record AddRecommendationUsageCommand(
	Guid RecommendationId,
	DateTime Date,
	string TimeOfDay) : ICommand;