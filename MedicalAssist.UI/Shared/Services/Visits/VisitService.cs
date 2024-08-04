using MedicalAssist.UI.Models.Visits;
using MedicalAssist.UI.Shared.Response;
using MedicalAssist.UI.Shared.Response.Base;
using MedicalAssist.UI.Shared.Services.Abstraction;

namespace MedicalAssist.UI.Shared.Services.Visits;

public class VisitService : IVisitService
{
	private readonly HttpClient _httpClient;

	public VisitService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response<List<VisitModel>>> GetAllVisits()
	{
		var response = await _httpClient.GetAsync("visit/");
		return await response.DeserializeResponse<List<VisitModel>>();
	}
}
