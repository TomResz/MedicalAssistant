using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Infrastructure.PDF.Reports;
using QuestPDF.Fluent;

namespace MedicalAssistant.Infrastructure.PDF.Services;

internal sealed class MedicalHistoryReportService : IMedicalHistoryReportPdfService
{
    private readonly IUserLanguageContext _userLanguageContext;
    
    public MedicalHistoryReportService(
        IUserLanguageContext userLanguageContext)
    {
        _userLanguageContext = userLanguageContext;
    }

    public PdfDto GenerateVisitReportPdf(List<MedicalHistoryDto> history)
    {
        var language = _userLanguageContext.GetLanguage();
        var report = new MedicalHistoryReport(language, history);
        var content = report.GeneratePdf();
        
        return new PdfDto()
        {
            Content = content,
            Name = language == Languages.Polish 
                ? $"historia_chorob_{DateTime.Now.ToString("HH_mm_dd_MM_yyyy")}.pdf" 
                : $"disease_history_{DateTime.Now.ToString("HH_mm_dd_MM_yyyy")}.pdf"
        };
    }
}