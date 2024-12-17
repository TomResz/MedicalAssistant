using MediatR;
using MedicalAssistant.Application.Dto;

namespace MedicalAssistant.Application.Reports.Medications;

public sealed record CreateMedicationReportCommand(List<Guid> IDs) : IRequest<PdfDto?>;