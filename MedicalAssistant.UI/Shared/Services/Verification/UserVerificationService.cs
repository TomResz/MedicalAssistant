using MedicalAssistant.UI.Shared.Requests;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using System.Net.Http.Json;

namespace MedicalAssistant.UI.Shared.Services.Verification;

public sealed class UserVerificationService : IUserVerificationService
{
	private readonly HttpClient _httpClient;

	public UserVerificationService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<Response.Base.Response> VerifyAccount(string code)
	{
		var response = await _httpClient.PutAsJsonAsync("user/verify", new AccountVerificationRequest { CodeHash = code });
		return await response.DeserializeResponse();
	}



	public async Task<Response.Base.Response> RegenerateCode(string email)
	{
		var response = await _httpClient.PutAsJsonAsync("user/regenerate-code", new CodeRegenerationRequest { Email = email });
		return await response.DeserializeResponse();
	}
}
