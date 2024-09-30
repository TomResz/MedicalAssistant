using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using System.Net.Http.Json;

namespace MedicalAssistant.UI.Shared.Services.Notification;

public class NotificationService : INotificationService
{
	private readonly HttpClient _httpClient;

	public NotificationService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response.Base.Response> MarkAsRead(List<Guid> ids)
	{
		var response = await _httpClient.PostAsJsonAsync("notification/mark-as-read", new {iDs = ids});
		return await response.DeserializeResponse();
	}

	public async Task<Response.Base.Response<List<NotificationModel>>> GetUnread()
	{
		var response = await _httpClient.GetAsync("notification/unread");
		return await response.DeserializeResponse<List<NotificationModel>>();
	}
}
