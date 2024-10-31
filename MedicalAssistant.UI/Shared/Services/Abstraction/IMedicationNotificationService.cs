using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Models.MedicationNotification;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IMedicationNotificationService
{
	Task<Response<DateRangeDto>> DateRange(Guid medicationId);
	Task<Response<List<MedicationNotificationWithDateRangeDto>>> Get(Guid medicationId, double hourOffset);
	Task<Response<Guid>> Add(AddMedicationNotificationModel model);
	Task<Response.Base.Response> Delete(Guid id);
	Task<Response.Base.Response> Edit(EditMedicationNotificationModel model);
}
