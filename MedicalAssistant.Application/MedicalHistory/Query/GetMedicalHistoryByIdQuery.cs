using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.MedicalHistory.Query;

public sealed record GetMedicalHistoryByIdQuery(
    Guid Id) : IRequest<MedicalHistoryDto>;