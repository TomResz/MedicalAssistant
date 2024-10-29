using MedicalAssistant.UI.Components.Medication;
using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;
public interface IMedicationService
{
    Task<Response<List<MedicationDto>>> GetAll();
    Task<Response<AddMedicationResponse>> Add(AddMedicationModel model);
	Task<Response<VisitDto?>> Update(UpdateMedicationModel request);
    Task<Response<List<MedicationDto>>> GetByDate(DateTime date);
    Task<Response.Base.Response> Delete(Guid id);
	Task<Response<List<MedicationWithDayDto>>> Week(DateTime date);
}