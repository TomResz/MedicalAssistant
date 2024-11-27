using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicalHistory.Query;

namespace MedicalAssistant.Application.Reports.MedicalHistory;

internal sealed class CreateMedicalHistoryReportCommandHandler
    : IRequestHandler<CreateMedicalHistoryReportCommand, PdfDto?>
{
    private readonly IMediator _mediator;
    private readonly IMedicalHistoryReportPdfService _medicalHistoryReportPdfService;

    public CreateMedicalHistoryReportCommandHandler(
        IMediator mediator,
        IMedicalHistoryReportPdfService medicalHistoryReportPdfService)
    {
        _mediator = mediator;
        _medicalHistoryReportPdfService = medicalHistoryReportPdfService;
    }

    public async Task<PdfDto?> Handle(CreateMedicalHistoryReportCommand request, CancellationToken cancellationToken)
    {
        var response = (await _mediator.Send(new GetMedicalHistoriesQuery(request.Ids), cancellationToken))
            .ToList();

        if (!response.Any())
        {
            return null;
        }

        var report = _medicalHistoryReportPdfService.GenerateVisitReportPdf(response);

        return report;
    }
}