using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalHistory.Query;

public record SearchMedicalHistoriesBySearchTermQuery(
    string SearchTerm) : IQuery<IEnumerable<MedicalHistoryDto>>;