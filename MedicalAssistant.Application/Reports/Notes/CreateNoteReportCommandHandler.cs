using MediatR;
using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicalNotes.Queries;

namespace MedicalAssistant.Application.Reports.Notes;

internal sealed class CreateNoteReportCommandHandler : ICommandHandler<CreateNoteReportCommand, PdfDto?>
{
    private readonly IMediator _mediator;
    private readonly IMedicalNoteReportPdfService _noteReportPdfService;
    public CreateNoteReportCommandHandler(
        IMediator mediator,
        IMedicalNoteReportPdfService noteReportPdfService)
    {
        _mediator = mediator;
        _noteReportPdfService = noteReportPdfService;
    }

    public async Task<PdfDto?> Handle(CreateNoteReportCommand request, CancellationToken cancellationToken)
    {
        var notes = await _mediator.Send(new GetMedicalNotesByIDsQuery(request.Ids), cancellationToken);

        if (notes is null || notes.Count == 0)
        {
            return null;
        }
        
        var pdf = _noteReportPdfService.GeneratePdf(notes);
        
        return pdf;
    }
}