using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Reports.Notes;

public sealed record CreateNoteReportCommand(
    List<Guid> Ids) : IRequest<PdfDto?>;