using MedicalAssistant.UI.Models.Login;
using MedicalAssistant.UI.Shared.Requests;
using MedicalAssistant.UI.Shared.Response;
using MedicalAssistant.UI.Shared.Response.Base;
using MedicalAssistant.UI.Shared.Services.Abstraction;
using System.Net.Http.Json;

namespace MedicalAssistant.UI.Shared.Services.User;

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

	public async Task<Response.Base.Response> Reactivate()
	{
		var response = await _httpClient.PostAsync("user/reactivate",null);
		return await response.DeserializeResponse();
	}

	public async Task<Response.Base.Response> DeactivateAccount()
	{
		var response = await _httpClient.PostAsync("user/deactivate",null);
		return await response.DeserializeResponse();
	}

	public async Task<Response.Base.Response> DeleteAccount()
	{
		var response = await _httpClient.DeleteAsync("user/");
		return await response.DeserializeResponse();
	}
}
