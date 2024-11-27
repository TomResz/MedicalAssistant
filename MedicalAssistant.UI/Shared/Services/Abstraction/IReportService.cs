﻿namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IReportService
{
    Task<bool> DownloadVisitReport(List<Guid> visitIds);
    Task<bool> DownloadMedicalHistoryReport(List<Guid> ids);
}