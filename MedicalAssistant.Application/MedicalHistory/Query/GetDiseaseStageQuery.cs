using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalHistory.Query;

public sealed record GetDiseaseStageQuery(
    Guid Id) : IQuery<DiseaseStageDto>;