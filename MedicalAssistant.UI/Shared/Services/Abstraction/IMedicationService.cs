using MedicalAssistant.UI.Components.Medication;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;
public interface IMedicationService
{
    Task<Response<List<MedicationDto>>> GetAll();
    Task<Response<AddMedicationResponse>> Add(AddMedicationModel model);
}