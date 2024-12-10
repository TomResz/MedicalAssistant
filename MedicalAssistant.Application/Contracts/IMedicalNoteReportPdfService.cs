using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Contracts;

public interface IMedicalNoteReportPdfService
{
    PdfDto GeneratePdf(List<MedicalNoteDto> noteDtos);
}