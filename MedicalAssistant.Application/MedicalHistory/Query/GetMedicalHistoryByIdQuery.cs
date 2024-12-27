using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalHistory.Query;

public sealed record GetMedicalHistoryByIdQuery(
    Guid Id) : IQuery<MedicalHistoryDto>;