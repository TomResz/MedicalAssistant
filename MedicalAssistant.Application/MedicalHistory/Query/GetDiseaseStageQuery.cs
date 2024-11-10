using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalHistory.Query;

public sealed record GetDiseaseStageQuery(
    Guid Id) : IRequest<DiseaseStageDto>;