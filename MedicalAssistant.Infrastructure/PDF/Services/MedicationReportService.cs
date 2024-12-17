using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Infrastructure.PDF.Reports;
using QuestPDF.Fluent;

namespace MedicalAssistant.Infrastructure.PDF.Services;

internal sealed class MedicationReportService : IMedicationReportService
{
    private readonly IUserLanguageContext _userLanguageContext;

    public MedicationReportService(IUserLanguageContext userLanguageContext)
    {
        _userLanguageContext = userLanguageContext;
    }

    public PdfDto GeneratePdf(List<MedicationRecommendationDto> medications)
    {
        var language = _userLanguageContext.GetLanguage();

        var pdf = new MedicationReport(language, medications);
        
        var content = pdf.GeneratePdf();
        
        return new PdfDto()
        {
            Content = content,
            Name = language == Languages.Polish 
                ? $"medykamenty_{DateTime.Now.ToString("HH_mm_dd_MM_yyyy")}.pdf" 
                : $"medications_{DateTime.Now.ToString("HH_mm_dd_MM_yyyy")}.pdf"
        };
    }
}