namespace MedicalAssistant.UI.Shared.Services.Abstraction;

public interface IMedicalHistoryService
{
    Task<Response.Base.Response<List<Models.MedicalHistory.MedicalHistoryDto>>> GetAll();
    Task<Response.Base.Response<List<Models.MedicalHistory.MedicalHistoryDto>>> GetByTerms(string searchTerm);
    Task<Response.Base.Response> Delete(Guid id);
}