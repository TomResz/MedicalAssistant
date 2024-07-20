using MedicalAssist.UI.Models.Visits;
using System.Text.Json;

namespace MedicalAssist.UI.Shared.Services.Visits;

public class VisitService
{
	private readonly HttpClient _httpClient;

	public VisitService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<List<VisitModel>?> GetAllVisits()
	{
		List<VisitModel> visits = new();
		var response = await _httpClient.GetAsync("visit/");

		if(response.IsSuccessStatusCode)
		{
			var json = await response.Content.ReadAsStringAsync();
			visits = JsonSerializer.Deserialize<List<VisitModel>>(json)!;
		}
		return visits;
	}
}
