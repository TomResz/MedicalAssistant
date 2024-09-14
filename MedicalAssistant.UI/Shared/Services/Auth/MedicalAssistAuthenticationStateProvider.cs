using MedicalAssistant.UI.Shared.Response;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedicalAssistant.UI.Shared.Services.Auth;

public class MedicalAssistAuthenticationStateProvider : AuthenticationStateProvider
{
	private readonly LocalStorageService _localStorage;

	public MedicalAssistAuthenticationStateProvider(LocalStorageService localStorage)
	{
		_localStorage = localStorage;
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var token = await _localStorage.GetValueAsync("access_token");
		var claims = new ClaimsIdentity();

		if (token is not null)
		{
			claims = GetClaimsFromJwtToken(token);	
		}
		var principal = new ClaimsPrincipal(claims);

		return new AuthenticationState(principal);
	}

	public static ClaimsIdentity GetClaimsFromJwtToken(string jwtToken)
	{
		var handler = new JwtSecurityTokenHandler();
		var token = handler.ReadJwtToken(jwtToken);

		var id = token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
		var fullName = token.Claims.First(x => x.Type == ClaimTypes.Name)!.Value;
		var email = token.Claims.First(x => x.Type == ClaimTypes.Email)!.Value;
		var role = token.Claims.First(x => x.Type == ClaimTypes.Role)!.Value;
		var isVerified = token.Claims.First(x => x.Type == "IsVerified")!.Value;
		var hasExternalProvider = token.Claims.First(x => x.Type == "HasExternalProvider")!.Value;

		return new ClaimsIdentity(new Claim[]
			{
				new(ClaimTypes.Email, email),
				new(ClaimTypes.NameIdentifier, id),
				new(ClaimTypes.Name, fullName),
				new (ClaimTypes.Role, role),
				new ("IsVerified",isVerified),
				new("HasExternalProvider",hasExternalProvider),
			}, "Api");
	}
	public async Task AuthenticateAsync(SignInResponse response)
	{
		await _localStorage.SetValueAsync("access_token", response.AccessToken);
		await _localStorage.SetValueAsync("refresh_token", response.RefreshToken);
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public async Task LogOutAsync()
	{
		await _localStorage.RemoveKeyAsync("access_token");
		await _localStorage.RemoveKeyAsync("refresh_token");
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}
}
