using System.Net.Http.Json;
using MedicalAssistant.UI.Models.MedicationUsage;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Services.Abstraction;

namespace MedicalAssistant.UI.Shared.Services.Medication;

public class MedicationUsageService : IMedicationUsageService
{
	private readonly HttpClient _httpClient;

	public MedicationUsageService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response.Base.Response<List<MedicationUsageDto>>> GetByDate(DateTime date)
	{
		var response = await _httpClient.GetAsync($"recommendation/usage/{date.Date.ToString("yyyy-MM-dd")}");
		return await response.DeserializeResponse<List<MedicationUsageDto>>();
	}

	public async Task<Response.Base.Response> Add(CreateMedicationUsage request)
	{
		var response = await _httpClient.PostAsJsonAsync($"recommendation/usage", request);
		return await response.DeserializeResponse();
	}
}
