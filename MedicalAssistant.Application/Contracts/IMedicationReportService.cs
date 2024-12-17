using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Contracts;

public interface IMedicationReportService
{
    PdfDto GeneratePdf(List<MedicationRecommendationDto> medications);

}