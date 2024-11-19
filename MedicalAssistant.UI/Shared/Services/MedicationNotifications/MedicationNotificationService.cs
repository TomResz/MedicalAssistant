using MedicalAssistant.UI.Models.Medication;
using MedicalAssistant.UI.Models.MedicationNotification;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using System.Net.Http.Json;
using System.Text.Json;

namespace MedicalAssistant.UI.Shared.Services.MedicationNotifications;

public class MedicationNotificationService
	: IMedicationNotificationService
{
	private readonly HttpClient _httpClient;
	private readonly static JsonSerializerOptions _options = new System.Text.Json.JsonSerializerOptions()
	{
		Converters = { new TimeOnlyJsonConverter() }
	};

	public MedicationNotificationService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response<Guid>> Add(AddMedicationNotificationModel model)
	{
		var response = await _httpClient.PostAsJsonAsync("recommendationNotification", model, options: _options); ;
		return await response.DeserializeResponse<Guid>();
	}

	public async Task<Response<DateRangeDto>> DateRange(Guid medicationId)
	{
		var response = await _httpClient.GetAsync($"recommendationNotification/dates/{medicationId}");
		return await response.DeserializeResponse<DateRangeDto>();
	}

	public async Task<Response<List<MedicationNotificationWithDateRangeDto>>> Get(Guid medicationId, double hourOffset)
	{
		var response = await _httpClient
			.GetAsync($"recommendationNotification/medication?id={medicationId}&offset={Math.Round(hourOffset, 1)}");
		return await response.DeserializeResponse<List<MedicationNotificationWithDateRangeDto>>();
	}

	public async Task<Response.Base.Response> Delete(Guid id)
	{
		var response = await _httpClient.DeleteAsync($"recommendationNotification/{id}");
		return await response.DeserializeResponse();
	}
	
	public async Task<Response.Base.Response> Edit(EditMedicationNotificationModel model)
	{
		var response = await _httpClient.PatchAsJsonAsync("recommendationNotification", model, _options);
		return await response.DeserializeResponse();
	}

	public async Task<Response<PagedList<MedicationNotificationPageContentDto>>> GetPagedList(int page, int pageSize, DateTime date, double offset)
	{
		var parameters = $"?page={page}&pageSize={pageSize}&date={date:yyyy-MM-dd}&offset={Math.Round(offset,1)}";
		var response = await _httpClient.GetAsync($"recommendationNotification{parameters}");
		return await response.DeserializeResponse<PagedList<MedicationNotificationPageContentDto>>();
	}
}