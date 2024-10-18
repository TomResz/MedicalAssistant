using MedicalAssistant.UI.Components.Medication;
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

	public async Task<Response<List<MedicationDto>>> GetAll()
	{
		var response = await _httpClient.GetAsync("recommendation/");
		return await response.DeserializeResponse<List<MedicationDto>>();
	}
}
