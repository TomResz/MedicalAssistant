using MedicalAssistant.UI.Components.VisitNotification;
using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Resources;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using System.Net.Http.Json;

namespace MedicalAssistant.UI.Shared.Services.Notifications;

public class VisitNotificationService : IVisitNotificationService
{
	private readonly HttpClient _httpClient;

	public VisitNotificationService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response<VisitNotificationDto>> Add(AddVisitNotificationModel model)
	{
		var response = await _httpClient.PostAsJsonAsync("visitnotification/", model);
		return await response.DeserializeResponse<VisitNotificationDto>();
	}

	public async Task<Response.Base.Response> ChangeDate(EditVisitNotificationModel model)
	{
		var response = await _httpClient.PutAsJsonAsync("visitnotification/date", model);
		return await response.DeserializeResponse();
	}

	public async Task<Response.Base.Response> Delete(Guid id)
	{
		var response = await _httpClient.DeleteAsync($"visitnotification/{id}");
		return await response.DeserializeResponse();
	}

	public async Task<Response<List<VisitNotificationDto>>> Get(Guid visitId)
	{
		var response = await _httpClient.GetAsync($"visitnotification/visit={visitId}");
		return await response.DeserializeResponse<List<VisitNotificationDto>>();
	}

	public async Task<Response<PagedList<VisitNotificationWithDetailsModel>>> GetPage(int page, int pageSize)
	{
		var response = await _httpClient.GetAsync($"visitnotification/upcoming?page={page}&pagesize={pageSize}");
		return await response.DeserializeResponse<PagedList<VisitNotificationWithDetailsModel>>();
	}

	public string MatchErrors(BaseErrorDetails errorDetails)
		=> errorDetails.Type switch
		{
			"ScheduledDateCannotBeGreatestThanDate" => Translations.ScheduledDateGreaterThanVisitDate,
			"InvalidVisitNotificationDate" => Translations.VisitAlreadyTakenPlace,
			_ => Translations.SomethingWentWrong
		};
}
