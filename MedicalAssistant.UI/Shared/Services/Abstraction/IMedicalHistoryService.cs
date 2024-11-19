using MedicalAssistant.UI.Components.MedicalHistory;
using MedicalAssistant.UI.Models.MedicalHistory;
using MedicalAssistant.UI.Shared.Response.Base;

namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IMedicalHistoryService
{
    Task<Response.Base.Response<List<Models.MedicalHistory.MedicalHistoryDto>>> GetAll();
    Task<Response.Base.Response<List<Models.MedicalHistory.MedicalHistoryDto>>> GetByTerms(string searchTerm);
    Task<Response.Base.Response> Delete(Guid id);
    Task<Response<Guid>> Add(MedicalHistoryViewModel viewModel);
    Task<Response<MedicalHistoryDto>> GetById(Guid id);
    Task<Response<Guid>> AddStage(AddDiseaseStageRequest request,Guid medicalHistoryId);
    Task<Response.Base.Response> Update(EditMedicalHistoryRequest request);
    Task<Response.Base.Response> DeleteStage(Guid id, Guid medicalHistoryId);
    Task<Response.Base.Response> EditStage(EditDiseaseStageRequest request, Guid medicalHistoryId);
}