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

	public async Task<Response<SignInResponse>> SignIn(LoginModel model)
	{
		var response = await _httpClient.PostAsJsonAsync("user/sign-in", model);
		if (response.IsSuccessStatusCode)
		{
			return new(await DeserializeOkResponse(response), true, Error.None);
		}
		var errorDetails = JsonSerializer.Deserialize<BaseErrorDetails>(await response.Content.ReadAsStringAsync())!;
		var error = MatchErrors(errorDetails);
		return new(null,false, error);
	}
	private async Task<SignInResponse> DeserializeOkResponse(HttpResponseMessage response)
	{
		var json = await response.Content.ReadAsStringAsync();
		var content = JsonSerializer.Deserialize<SignInResponse>(json)!;
		return content;
	}
	public async Task<Response.Response> SignUp(SignUpRequest request)
	{
		var response = await _httpClient.PostAsJsonAsync("user/sign-up", request);
		if (response.IsSuccessStatusCode)
		{
			return new(true, Error.None);
		}
		var errorDetails = JsonSerializer.Deserialize<BaseErrorDetails>(await response.Content.ReadAsStringAsync())!;
		return new(false,MatchErrors(errorDetails));
	}

	private static Error MatchErrors(BaseErrorDetails baseError)
	{
		return baseError.Type switch
		{
			"InvalidLoginProviderException" => AuthErrors.InvalidLoginProvider,
			"InvalidSignInCredentialsException" => AuthErrors.InvalidSignInCredentials,
			"UnverifiedUserException" => AuthErrors.UnverifiedUser,
			"InvalidExternalProviderException" => AuthErrors.InvalidExternalProvider,
			"EmailInUseException" => AuthErrors.EmailInUse,
			_ => Error.InternalServerError
		};
	}

}
