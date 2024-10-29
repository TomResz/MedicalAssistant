using MedicalAssistant.UI.Components.Medication;
using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using System.Net.Http.Json;

namespace MedicalAssistant.UI.Shared.Services.Medication;

public class MedicationService : IMedicationService
{
	private readonly HttpClient _httpClient;

	public MedicationService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response<AddMedicationResponse>> Add(AddMedicationModel model)
	{
		var response = await _httpClient.PostAsJsonAsync("recommendation/add", model);
		return await response.DeserializeResponse<AddMedicationResponse>();
	}

	public async Task<Response.Base.Response> Delete(Guid id)
	{
		var response = await _httpClient.DeleteAsync($"recommendation/{id}");
		return await response.DeserializeResponse();
	}

	public async Task<Response<List<MedicationDto>>> GetAll()
	{
		var response = await _httpClient.GetAsync("recommendation/");
		return await response.DeserializeResponse<List<MedicationDto>>();
	}

    public async Task<Response<List<MedicationDto>>> GetByDate(DateTime date)
    {
		var response = await _httpClient.GetAsync($"recommendation/{date.ToString("yyyy-MM-dd")}");
		return await response.DeserializeResponse<List<MedicationDto>>();
    }

    public async Task<Response<VisitDto?>> Update(UpdateMedicationModel request)
	{
		var response = await _httpClient.PatchAsJsonAsync("recommendation", request);
		return await response.DeserializeResponse<VisitDto?>();
	}

	public async Task<Response<List<MedicationWithDayDto>>> Week(DateTime date)
	{
		var response = await _httpClient.GetAsync($"recommendation/week/{date:yyyy-MM-dd}");
		return await response.DeserializeResponse<List<MedicationWithDayDto>>();	
	}
}
