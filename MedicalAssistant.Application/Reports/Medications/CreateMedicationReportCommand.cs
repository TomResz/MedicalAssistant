using MedicalAssistant.Application.Abstraction;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Reports.Medications;

public sealed record CreateMedicationReportCommand(List<Guid> IDs) : ICommand<PdfDto?>;