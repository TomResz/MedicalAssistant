using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicationRecommendations.Queries;

public sealed record GetMedicationDateRangeQuery(
	Guid Id) : IRequest<DateRangeDto?>;