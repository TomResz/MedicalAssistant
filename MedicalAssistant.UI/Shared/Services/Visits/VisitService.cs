using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using System.Net.Http.Json;

namespace MedicalAssistant.UI.Shared.Services.Visits;

public class VisitService : IVisitService
{
	private readonly HttpClient _httpClient;

	public VisitService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response<List<VisitDto>>> GetAllVisits()
	{
		var response = await _httpClient.GetAsync("visit/");
		return await response.DeserializeResponse<List<VisitDto>>();
	}

	public async Task<Response<VisitDto>> Add(CreateVisitVisitModel model)
	{
		var response = await _httpClient.PostAsJsonAsync("visit/add", model);
		return await response.DeserializeResponse<VisitDto>();
	}

	public async Task<Response.Base.Response> Delete(Guid visitId)
	{
		var response = await _httpClient.DeleteAsync($"visit/delete/{visitId}");
		return await response.DeserializeResponse();
	}

	public async Task<Response<VisitDto>> Edit(EditVisitModel editVisitModel)
	{
		var response = await _httpClient.PutAsJsonAsync("visit/edit", editVisitModel);
		return await response.DeserializeResponse<VisitDto>();
	}

	public async Task<Response<VisitDto>> Get(Guid visitId)
	{
		var response = await _httpClient.GetAsync($"visit/{visitId}");
		return await response.DeserializeResponse<VisitDto>();
	}
}
