using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Commands.UpdateRecommendation;
public sealed record UpdateMedicationRecommendationCommand(
	Guid Id,
	Guid? VisitId,
	string? ExtraNote,
	string MedicineName,
	int Quantity,
	string[] TimeOfDay,
	DateTime StartDate,
	DateTime EndDate) : IRequest<VisitDto?>;
