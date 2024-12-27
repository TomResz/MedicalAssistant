using MediatR;
using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.Visits.Queries;

namespace MedicalAssistant.Application.Reports.Visit;

internal sealed class CreateVisitReportCommandHandler : ICommandHandler<CreateVisitReportCommand, PdfDto?>
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
        var response = await _mediator.Send(new GetVisitsWithRecommendationsQuery(request.Ids), cancellationToken);

        var recommendations = response.ToList();
        
        if (recommendations is null || recommendations.Count == 0)
        {
            return null;
        }
        
        var pdf = _visitReportPdfService.GenerateVisitReportPdf(recommendations);
        return pdf;
    }
}