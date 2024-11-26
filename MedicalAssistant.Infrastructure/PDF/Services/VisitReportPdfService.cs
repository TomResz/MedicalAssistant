using MedicalAssistant.Application.Contracts;
using MedicalAssistant.Application.Dto;
using MedicalAssistant.Domain.Enums;
using MedicalAssistant.Infrastructure.PDF.Reports;
using QuestPDF.Fluent;

namespace MedicalAssistant.Infrastructure.PDF.Services;

internal sealed class VisitReportPdfService : IVisitReportPdfService
{
    private readonly IUserLanguageContext _userLanguageContext;

    public VisitReportPdfService(IUserLanguageContext userLanguageContext)
    {
        _userLanguageContext = userLanguageContext;
    }

    public PdfDto GenerateVisitReportPdf(List<VisitWithRecommendationsDto> visits)
    {
        var language = _userLanguageContext.GetLanguage();
        var pdf = new VisitReport(language, visits);
       var pdfByteContent =  pdf.GeneratePdf();
       
       return new PdfDto
       {
           Content = pdfByteContent,
           Name = language == Languages.Polish
               ? $"raport_medyczny_{DateTime.Now}.pdf" 
               : $"medical_raport_{DateTime.Now}.pdf"
       };
    }

}