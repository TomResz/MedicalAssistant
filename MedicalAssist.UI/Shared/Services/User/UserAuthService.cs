using MedicalAssist.UI.Models.Login;
using MedicalAssist.UI.Shared.Response;
using MedicalAssist.UI.Shared.Services.Abstraction;
using System.Net.Http.Json;
using System.Text.Json;

namespace MedicalAssist.UI.Shared.Services.User;

internal sealed class UserAuthService : IUserAuthService
{
	private readonly HttpClient _httpClient;

	public UserAuthService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<SignInResponse> SignInByFacebook(string code)
	{
		var response = await _httpClient.PostAsJsonAsync("user/login-facebook", new { code = code });
		if (response.IsSuccessStatusCode)
		{
			return await DeserializeOkResponse(response);
		}

		return null;
	}
	public async Task<SignInResponse> SignInByGoogle(string code)
	{
		var response = await _httpClient.PostAsJsonAsync("user/login-google", new { code = code });
		if (response.IsSuccessStatusCode)
		{
			return await DeserializeOkResponse(response);
		}

		return null;
	}

	public async Task<SignInResponse> SignIn(LoginModel model)
	{
		var response = await _httpClient.PostAsJsonAsync("user/sign-in", model);
		if (response.IsSuccessStatusCode)
		{
			return await DeserializeOkResponse(response);
		}

		return null;
	}

	private async Task<SignInResponse> DeserializeOkResponse(HttpResponseMessage response)
	{
		var json = await response.Content.ReadAsStringAsync();
		var content = JsonSerializer.Deserialize<SignInResponse>(json)!;
		return content;
	}




}
