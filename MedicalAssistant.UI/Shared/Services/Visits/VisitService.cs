using MedicalAssistant.UI.Models.Visits;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using Microsoft.Extensions.FileProviders;
using System.Net.Http.Json;
using System.Web;

namespace MedicalAssistant.UI.Shared.Services.Visits;

public class VisitService : IVisitService
{
	private readonly HttpClient _httpClient;

	public VisitService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response<List<VisitDto>>> GetAllVisits(string? direction = null)
	{
		string uri = "visit";

		if(direction is not null)
		{
			uri = $"visit?direction={direction}";
		}

		var response = await _httpClient.GetAsync(uri);
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

	public async Task<Response<List<VisitDto>>> GetByWeek(DateTime dateTime)
	{ 
		var response = await _httpClient.GetAsync($"visit/week/{dateTime.ToString("yyyy-MM-dd")}");
		return await response.DeserializeResponse<List<VisitDto>>();	
	}

	public async Task<Response<List<VisitDto>>> GetCompleted(DateTime dateTime)
	{
		var date = dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
		var response = await _httpClient.GetAsync($"visit/completed/{date}");
		return await response.DeserializeResponse<List<VisitDto>>();
	}

	public async Task<Response<List<VisitDto>>> GetByDate(DateTime date)
	{
		var response = await _httpClient.GetAsync($"visit/{date.ToString("yyyy-MM-dd")}");
		return await response.DeserializeResponse<List<VisitDto>>();
	}

	public async Task<Response<List<VisitDto>>> GetBySearchTerm(string searchTerm,string? direction = null)
	{
		string dir = string.Empty;
		if(!string.IsNullOrEmpty(direction))
		{
			dir = $"?direction={direction}";
		}
		var httpCodedText = HttpUtility.UrlEncode($"{searchTerm}{dir}");
		var response = await _httpClient.GetAsync($"visit/search/{httpCodedText}");
		return await response.DeserializeResponse<List<VisitDto>>();
	}
}
