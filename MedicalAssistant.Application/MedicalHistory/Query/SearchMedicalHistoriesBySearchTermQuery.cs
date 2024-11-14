using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalHistory.Query;

public record SearchMedicalHistoriesBySearchTermQuery(
    string SearchTerm) : IRequest<IEnumerable<MedicalHistoryDto>>;