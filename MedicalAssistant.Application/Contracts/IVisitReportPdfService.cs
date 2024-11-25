using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Contracts;

public interface IVisitReportPdfService
{
    PdfDto GenerateVisitReportPdf(List<VisitWithRecommendationsDto> visits);
}