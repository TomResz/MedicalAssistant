namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IReportService
{
    Task<bool> DownloadVisitReport(List<Guid> visitIds);
    Task<bool> DownloadMedicalHistoryReport(List<Guid> ids);
    Task<bool> DownloadNoteReport(List<Guid> ids);
    Task<bool> DownloadMedicationReport(List<Guid> ids);
}