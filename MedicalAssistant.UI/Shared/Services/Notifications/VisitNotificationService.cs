using MedicalAssistant.UI.Models.Notifications;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;

namespace MedicalAssistant.UI.Shared.Services.Notifications;

public class VisitNotificationService : IVisitNotificationService
{
	private readonly HttpClient _httpClient;

	public VisitNotificationService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response<List<VisitNotificationDto>>> Get(Guid visitId)
	{
		var response = await _httpClient.GetAsync($"visitnotification/visit={visitId}");
		return await response.DeserializeResponse<List<VisitNotificationDto>>();
	}
}
