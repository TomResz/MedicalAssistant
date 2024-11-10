using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalHistory.Query;

public sealed record GetDiseaseStagesQuery(
    Guid MedicalHistoryId) : IRequest<IEnumerable<DiseaseStageDto>>;