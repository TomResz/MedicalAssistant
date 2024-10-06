using MedicalAssistant.UI.Models.Settings;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using System.Net.Http.Json;

namespace MedicalAssistant.UI.Shared.Services.Settings;

public sealed class SettingsService : ISettingsService
{
	private readonly HttpClient _httpClient;

	public SettingsService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response<SettingsViewModel>> Get()
	{
		var response = await _httpClient.GetAsync("settings/");
		return await response.DeserializeResponse<SettingsViewModel>();
	}

	public async Task<Response.Base.Response> Update(SettingsViewModel model)
	{
		var response = await _httpClient.PatchAsJsonAsync("settings/update", model);
		return await response.DeserializeResponse();
	}
}
