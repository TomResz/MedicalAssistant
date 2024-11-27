using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalHistory.Query;

public sealed record GetMedicalHistoriesQuery(List<Guid>? Ids = null)
    : IRequest<IEnumerable<MedicalHistoryDto>>;