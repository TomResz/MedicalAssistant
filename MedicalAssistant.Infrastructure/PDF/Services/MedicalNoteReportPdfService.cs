using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Infrastructure.PDF.Reports;
using QuestPDF.Fluent;

namespace MedicalAssistant.Infrastructure.PDF.Services;

internal sealed class MedicalNoteReportPdfService : IMedicalNoteReportPdfService
{
    private readonly IUserLanguageContext _userLanguageContext;

    public MedicalNoteReportPdfService(IUserLanguageContext userLanguageContext)
    {
        _userLanguageContext = userLanguageContext;
    }

    public PdfDto GeneratePdf(List<MedicalNoteDto> noteDtos)
    {
        var language = _userLanguageContext.GetLanguage();
        var report = new MedicalNoteReport(language, noteDtos);
        
        var content = report.GeneratePdf();
        
        return new PdfDto()
        {
            Content = content,
            Name = language == Languages.Polish 
                ? $"notatki_{DateTime.Now.ToString("HH_mm_dd_MM_yyyy")}.pdf" 
                : $"notes_{DateTime.Now.ToString("HH_mm_dd_MM_yyyy")}.pdf"
        };

    }
}