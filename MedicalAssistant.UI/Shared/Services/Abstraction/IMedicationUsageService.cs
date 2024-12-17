using MedicalAssistant.UI.Models.MedicationUsage;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;
public interface IMedicationUsageService
{
    Task<Response<List<MedicationUsageDto>>> GetByDate(DateTime date);
    Task<Response.Base.Response> Add(CreateMedicationUsage request);
}