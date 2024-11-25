using MediatR;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Visits.Queries;

namespace MedicalAssistant.Application.Reports.Visit;

public sealed record CreateVisitReportCommand() : IRequest<PdfDto?>;

internal sealed class CreateVisitReportCommandHandler : IRequestHandler<CreateVisitReportCommand, PdfDto?>
{
    private readonly IVisitReportPdfService _visitReportPdfService;
    private readonly IMediator _mediator;

    public CreateVisitReportCommandHandler(
        IVisitReportPdfService visitReportPdfService,
        IMediator mediator)
    {
        _visitReportPdfService = visitReportPdfService;
        _mediator = mediator;
    }

    public async Task<PdfDto?> Handle(CreateVisitReportCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetVisitsWithRecommendationsQuery());
        var pdf = _visitReportPdfService.GenerateVisitReportPdf(response.ToList());
        return pdf;
    }
}