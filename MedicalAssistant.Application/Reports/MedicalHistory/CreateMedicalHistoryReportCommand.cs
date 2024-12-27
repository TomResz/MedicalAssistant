using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Reports.MedicalHistory;

public sealed record CreateMedicalHistoryReportCommand(
    List<Guid> Ids) : ICommand<PdfDto?>;