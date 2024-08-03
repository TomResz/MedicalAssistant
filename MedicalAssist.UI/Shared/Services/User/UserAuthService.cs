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

	public async Task<Response<SignInResponse>> SignInByFacebook(string code)
	{
		var response = await _httpClient.PostAsJsonAsync("user/login-facebook", new { code = code });
		return await DeserializeResponse(response);
	}
	public async Task<Response<SignInResponse>> SignInByGoogle(string code)
	{
		var response = await _httpClient.PostAsJsonAsync("user/login-google", new { code = code });
		return await DeserializeResponse(response);
	}

	public async Task<Response<SignInResponse>> SignIn(LoginModel model)
	{
		var response = await _httpClient.PostAsJsonAsync("user/sign-in", model);
		return await DeserializeResponse(response);
	}

	public async Task<Response.Response> SignUp(SignUpRequest request)
	{
		var response = await _httpClient.PostAsJsonAsync("user/sign-up", request);
		if (response.IsSuccessStatusCode)
		{
			return new(true);
		}
		var errorDetails = JsonSerializer.Deserialize<BaseErrorDetails>(await response.Content.ReadAsStringAsync())!;
		return new(false, errorDetails);
	}



	private async static Task<Response<SignInResponse>> DeserializeResponse(HttpResponseMessage httpResponse)
	{
		if (httpResponse.IsSuccessStatusCode)
		{
			var json = await httpResponse.Content.ReadAsStringAsync();
			var response = JsonSerializer.Deserialize<SignInResponse>(json)!;
			return new(response, true);
		}
		var errorDetails = JsonSerializer.Deserialize<BaseErrorDetails>(await httpResponse.Content.ReadAsStringAsync())!;
		return new(null, false, errorDetails);
	}

}
