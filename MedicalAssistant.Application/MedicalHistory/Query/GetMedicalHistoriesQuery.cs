using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalHistory.Query;

public sealed record GetMedicalHistoriesQuery()
    : IRequest<IEnumerable<MedicalHistoryDto>>;