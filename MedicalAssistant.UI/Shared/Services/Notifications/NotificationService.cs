using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using MudBlazor;
using System.Net.Http.Json;

namespace MedicalAssistant.UI.Shared.Services.Notifications;

public class NotificationService : INotificationService
{
	private readonly HttpClient _httpClient;

	public NotificationService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response.Base.Response> MarkAsRead(List<Guid> ids)
	{
		var response = await _httpClient.PostAsJsonAsync("notification/mark-as-read", new { iDs = ids });
		return await response.DeserializeResponse();
	}

	public async Task<Response.Base.Response<List<NotificationModel>>> GetUnread()
	{
		var response = await _httpClient.GetAsync("notification/unread");
		return await response.DeserializeResponse<List<NotificationModel>>();
	}

	public async Task<Response<PagedList<NotificationModel>>> GetPage(int page, int pageSize)
	{
		var response = await _httpClient.GetAsync($"notification?page={page}&pagesize={pageSize}");
		return await response.DeserializeResponse<PagedList<NotificationModel>>();
	}

	public async Task<Response<List<NotificationModel>>> Get()
	{
		var response = await _httpClient.GetAsync("notification/all");
		return await response.DeserializeResponse<List<NotificationModel>>();
	}
}
