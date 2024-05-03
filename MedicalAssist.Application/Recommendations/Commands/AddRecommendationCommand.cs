using MediatR;
using MedicalAssist.Domain.ValueObjects.IDs;

namespace MedicalAssist.Application.Recommendations.Commands;
public sealed record AddRecommendationCommand(
	Guid VisitId,
	string? ExtraNote,
	string MedicineName,
	int Quantity,
	string TimeOfDay) : IRequest;
