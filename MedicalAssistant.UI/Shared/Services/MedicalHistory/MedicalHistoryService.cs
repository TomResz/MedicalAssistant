using System.Web;
using MedicalAssistant.UI.Models.MedicalHistory;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;

namespace MedicalAssistant.UI.Shared.Services.MedicalHistory;

public class MedicalHistoryService : IMedicalHistoryService
{
    private readonly HttpClient _httpClient;

    public MedicalHistoryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Response.Base.Response<List<Models.MedicalHistory.MedicalHistoryDto>>> GetAll()
    {
        var response = await _httpClient.GetAsync("medicalhistory/");
        return await response.DeserializeResponse<List<Models.MedicalHistory.MedicalHistoryDto>>();
    }

    public async Task<Response<List<MedicalHistoryDto>>> GetByTerms(string searchTerm)
    {
        var encodedStr = HttpUtility.UrlEncode(searchTerm);
        var response = await _httpClient.GetAsync("medicalhistory/");
        return await response.DeserializeResponse<List<Models.MedicalHistory.MedicalHistoryDto>>();
    }

    public async Task<Response.Base.Response> Delete(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"medicalhistory/{id}");
        return await response.DeserializeResponse<Response.Base.Response>();
    }
}