using MediatR;
using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Application.MedicationRecommendations.Queries;

namespace MedicalAssistant.Application.Reports.Medications;

internal sealed class CreateMedicationReportCommandHandler : ICommandHandler<CreateMedicationReportCommand, PdfDto?>
{
    private readonly IMedicationReportService _medicationReportService;
    private readonly IMediator _mediator;

    public CreateMedicationReportCommandHandler(
        IMedicationReportService medicationReportService,
        IMediator mediator)
    {
        _medicationReportService = medicationReportService;
        _mediator = mediator;
    }

    public async Task<PdfDto?> Handle(CreateMedicationReportCommand request, CancellationToken cancellationToken)
    {
        var query = new GetByIdsMedicationRecommendationsQuery(request.IDs);
        var medicationRecommendations = await _mediator.Send(query);

        var medicationRecommendationDtos = medicationRecommendations.ToList();
        
        if (!medicationRecommendationDtos.Any())
        {
            return null;
        }
        
        var pdf = _medicationReportService.GeneratePdf(medicationRecommendationDtos);
        
        return pdf;
    }
}