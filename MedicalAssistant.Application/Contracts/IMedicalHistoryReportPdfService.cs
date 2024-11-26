using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Contracts;

public interface IMedicalHistoryReportPdfService
{
    PdfDto GenerateVisitReportPdf(List<MedicalHistoryDto> history);
}