using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalHistory.Query;

public sealed record GetDiseaseStagesQuery(
    Guid MedicalHistoryId) : IQuery<IEnumerable<DiseaseStageDto>>;