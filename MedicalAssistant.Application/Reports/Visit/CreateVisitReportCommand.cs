using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Reports.Visit;

public sealed record CreateVisitReportCommand() : IRequest<PdfDto?>;