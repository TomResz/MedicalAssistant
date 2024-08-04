using MedicalAssist.UI.Models.Login;
using MedicalAssist.UI.Shared.Requests;
using MedicalAssist.UI.Shared.Response;
using MedicalAssist.UI.Shared.Response.Base;
using MedicalAssist.UI.Shared.Services.Abstraction;
using System.Net.Http.Json;

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
		var response = await _httpClient.PostAsJsonAsync("user/login-facebook", new SignInByExternalProviderRequest { Code = code});
		return await response.DeserializeResponse<SignInResponse>();
	}
	public async Task<Response<SignInResponse>> SignInByGoogle(string code)
	{
		var response = await _httpClient.PostAsJsonAsync("user/login-google", new SignInByExternalProviderRequest { Code = code });
		return await response.DeserializeResponse<SignInResponse>();
	}

	public async Task<Response<SignInResponse>> SignIn(LoginModel model)
	{
		var response = await _httpClient.PostAsJsonAsync("user/sign-in", model);
		return await response.DeserializeResponse<SignInResponse>();
	}

	public async Task<Response.Base.Response> SignUp(SignUpRequest request)
	{
		var response = await _httpClient.PostAsJsonAsync("user/sign-up", request);
		return await response.DeserializeResponse();
	}
}
