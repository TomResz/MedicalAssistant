using System.Net.Http.Json;

namespace MedicalAssist.UI.Shared.Services.Verification;

public sealed class UserVerificationService : IUserVerificationService
{
	private readonly HttpClient _httpClient;

	public UserVerificationService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<bool> VerifyAccount(string code)
	{
		var response = await _httpClient.PutAsJsonAsync("user/verify", new { codeHash = code });
		return response.IsSuccessStatusCode;
	}
}
